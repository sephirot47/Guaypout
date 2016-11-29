using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackBuilder : MonoBehaviour 
{
    public float trackScale;

    public int numPieces;
    public GameObject player;
    public TrackPiece initialPiece;
    public TrackPiece[] trackPiecesPrefabs;

    private List<TrackPiece> trackPieces;

    void Start () 
    {
        trackPieces = new List<TrackPiece>();
        while (trackPieces.Count <= numPieces)
        {
            AddRandomPiece();
        }
    }

    void Update () 
    {
    }

    void AddRandomPiece()
    {
        int maximumTries = 10; // Just in case to avoid infinite loops
        bool foundCompatibleTrackPiece = false;
        GameObject newTrackPieceGO = null;
        while (--maximumTries != 0)
        {
            if (trackPieces.Count > 0)
            {
                newTrackPieceGO = GameObject.Instantiate(GetRandomTrackPiecePrefab()) as GameObject;
            }
            else
            {
                newTrackPieceGO = GameObject.Instantiate(initialPiece.gameObject) as GameObject;
            }
            newTrackPieceGO.transform.localScale = Vector3.one * trackScale;

            TrackPiece newTrackPiece = newTrackPieceGO.GetComponent<TrackPiece>();
            if (trackPieces.Count > 0)
            {
                // Align with the previous track piece
                TrackPiece prevTrackPiece = trackPieces[trackPieces.Count - 1];
                newTrackPiece.ConcatenateWithPreviousTrackPiece(prevTrackPiece);
            }
            else // First piece
            {
                newTrackPiece.transform.position = player.transform.position - player.transform.up * 4;
                newTrackPiece.transform.LookAt(newTrackPiece.transform.position + Vector3.forward, Vector3.up);
            }

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
            Debug.DebugBreak();
        }

        trackPieces.Add(newTrackPieceGO.GetComponent<TrackPiece>());

        if (!foundCompatibleTrackPiece)
        {
            Debug.LogWarning("HAVENT FOUND COMPATIBLE TRACK PIECE, backtracking");
            Backtrack();
        }
    }

    void BacktrackPiece(int i)
    {
        if (i >= 0 && i < trackPieces.Count)
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
        return trackPiecesPrefabs[Random.Range(0, trackPiecesPrefabs.Length)].gameObject;
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
