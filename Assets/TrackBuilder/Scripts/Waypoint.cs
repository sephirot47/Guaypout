using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour 
{
    private TrackPiece parentTrackPiece;

	void Awake () 
    {
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }

        parentTrackPiece = GetComponentInParent<TrackPiece>();
	}
	
	void Update () {
	
	}

    // Returns the track piece the waypoint is in
    public TrackPiece GetTrackPiece()
    {
        return parentTrackPiece;
    }

    public Vector3 GetForward()
    {
        return transform.forward;
    }
}
