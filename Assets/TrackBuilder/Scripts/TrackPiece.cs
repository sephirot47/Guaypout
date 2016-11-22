using UnityEngine;
using System.Collections;

public class TrackPiece : MonoBehaviour 
{
    [SerializeField]
    private Waypoint[] waypoints;

	void Start () 
    {
	}
	
	void Update () 
    {
	}

    public void ConcatenateWithPreviousTrackPiece(TrackPiece previousTrackPiece)
    {
        Waypoint firstWaypoint = GetFirstWaypoint();
        Waypoint prevLastWaypoint = previousTrackPiece.GetLastWaypoint();
        transform.rotation = Quaternion.FromToRotation(firstWaypoint.transform.forward,
                                                       prevLastWaypoint.transform.forward);
        transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up); // Always looking to up
        transform.position = prevLastWaypoint.transform.position + (transform.position - firstWaypoint.transform.position);
    }

    public Waypoint GetFirstWaypoint()
    {
        return waypoints[0];
    }

    public Waypoint GetLastWaypoint()
    {
        return waypoints[waypoints.Length - 1];
    }
}
