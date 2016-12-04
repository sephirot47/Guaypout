using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackBuilder : MonoBehaviour 
{
    public float trackScale;

    public TrackPiece initialPiece;
    public TrackPiece lastTrackPiecePrefab;
    public TrackPiece[] trackPiecesPrefabs;

    [HideInInspector]
    public int numPieces;
    public float[] piecesProbabilities;

    private List<TrackPiece> trackPieces;

    void Awake () 
    {
        trackPieces = new List<TrackPiece>();
        piecesProbabilities = NormalizedProbabilities(new float[] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f});
        GenerateTrack();
    }

    public float[] NormalizedProbabilities(float[] probs)
    {
        float totalProb = 0.0f;
        for (int i = 0; i < probs.Length; ++i)
        {
            totalProb += probs[i];
        }
        for (int i = 0; i < probs.Length; ++i)
        {
            probs[i] /= totalProb;
        }

        return probs;
    }

    public void ClearTrack()
    {
        for (int i = 1; i < trackPieces.Count; ++i)
        {
            GameObject.DestroyObject(trackPieces[i].gameObject);
        }
        trackPieces.Clear();
    }

    public void GenerateTrack(int seed = -1)
    {
        if (seed != -1)
        {
            // Called from track creator
            Random.InitState(seed); 
        }
        else
        {
            // Called from InRace scene.
            // Get all the used parameters in the track creator to create the exact same track.
            Random.InitState(PreviewTrackController.lastUsedRandomSeed);
            piecesProbabilities[0] = piecesProbabilities[1] = PreviewTrackController.curveProbabilities;
            piecesProbabilities[2] = piecesProbabilities[3] = PreviewTrackController.slopesProbabilities;
            piecesProbabilities[4] = piecesProbabilities[5] = PreviewTrackController.straightProbabilities;
            numPieces = PreviewTrackController.numPieces;
        }

        ClearTrack();
        piecesProbabilities = NormalizedProbabilities(piecesProbabilities);
        trackPieces.Add(initialPiece);
        while (trackPieces.Count <= numPieces)
        {
            AddRandomPiece();
        }
    }

    void AddRandomPiece()
    {
        int maximumTries = 10; // Just in case to avoid infinite loops
        bool foundCompatibleTrackPiece = false;
        GameObject newTrackPieceGO = null;
        while (--maximumTries != 0)
        {
            if (trackPieces.Count <= numPieces - 1)
            {
                newTrackPieceGO = GameObject.Instantiate(GetRandomTrackPiecePrefab()) as GameObject;
            }
            else
            {
                newTrackPieceGO = GameObject.Instantiate(lastTrackPiecePrefab.gameObject) as GameObject;
            }
            newTrackPieceGO.transform.localScale = Vector3.one * trackScale;

            TrackPiece newTrackPiece = newTrackPieceGO.GetComponent<TrackPiece>();
            // Align with the previous track piece
            TrackPiece prevTrackPiece = trackPieces[trackPieces.Count - 1];
            newTrackPiece.ConcatenateWithPreviousTrackPiece(prevTrackPiece);

            if (CollidesWithTrack(newTrackPieceGO))
            {
                if (maximumTries > 1) // Dont destroy in the last try
                {
                    Destroy(newTrackPieceGO);
                }
            }
            else // We have found a good candidate for track piece!
            {
                foundCompatibleTrackPiece = true;
                newTrackPiece.transform.parent = transform;
                break;
            }
        }

        trackPieces.Add(newTrackPieceGO.GetComponent<TrackPiece>());

        if (!foundCompatibleTrackPiece)
        {
            //Debug.LogWarning("HAVENT FOUND COMPATIBLE TRACK PIECE, backtracking");
            Backtrack();
        }
    }

    void BacktrackPiece(int i)
    {
        if (i > 0 && i < trackPieces.Count)
        {
            Destroy(trackPieces[i].gameObject);
            trackPieces.RemoveAt(i);
        }
    }

    void Backtrack()
    {
        for (int i = 0; i < 10; ++i)
        {
            BacktrackPiece(trackPieces.Count - 1);
        }
    }

    // Checks whether the passed piece collides with the rest of the track (except the last piece) or not
    bool CollidesWithTrack(GameObject newTrackPiece)
    {
        foreach (TrackPiece trackPiece in trackPieces)
        {
            // Ignore last track piece, since they will be always colliding
            if (trackPiece == trackPieces[trackPieces.Count - 1]) continue; 
            
            foreach (Collider col in trackPiece.GetComponentsInChildren<Collider>())
            {
                foreach (Collider col2 in newTrackPiece.GetComponentsInChildren<Collider>())
                {
                    if (col.bounds.Intersects(col2.bounds))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private GameObject GetRandomTrackPiecePrefab()
    {
        float cumulativeProbability = 0.0f;
        for (int i = 0; i < trackPiecesPrefabs.Length; i++)
        {
            cumulativeProbability += piecesProbabilities[i];
            if (Random.Range(0.0f, 1.0f) < cumulativeProbability)
            {
                return trackPiecesPrefabs[i].gameObject;
            }
        }
        return null;
    }

	public List<Waypoint> GetWaypointsList() // Returns the ordered list of waypoints
	{
		List<Waypoint> wpList = new List<Waypoint> ();
		foreach (TrackPiece tp in trackPieces) {
			foreach (Waypoint wp in tp.GetWaypoints()) {
				wpList.Add (wp);
			}
		}
		return wpList;
	}

	public Waypoint GetClosestWaypoint(Vector3 position)
	{
		Waypoint closestWaypoint = null;
		float closestDistance = Mathf.Infinity;
		List<Waypoint> waypoints = GetWaypointsList();
		foreach (Waypoint wp in waypoints) 
		{
			float d = Vector3.Distance (position, wp.transform.position);
			if (d < closestDistance) 
			{
				closestWaypoint = wp;
				closestDistance = d;
			}
		}
		return closestWaypoint;
	}

    public List<TrackPiece> GetTrackPieces()
    {
        return trackPieces;
    }
}
