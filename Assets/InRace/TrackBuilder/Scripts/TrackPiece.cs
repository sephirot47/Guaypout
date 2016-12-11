using UnityEngine;
using System.Collections;

public class TrackPiece : MonoBehaviour 
{
    public GameObject itemOrbRow;
    public GameObject speedBoosterPrefab;

    [SerializeField]
    private Waypoint[] waypoints;

    [SerializeField]
    [Range (0,1)]
    private float speedBoosterChance;
    private GameObject speedBooster;

	void Start () 
    {
        if (Random.Range(0.0f, 1.0f) < speedBoosterChance)
        {
            speedBooster = GameObject.Instantiate(
                speedBoosterPrefab, 
                waypoints[0].transform.position + transform.up * 1.5f,
                waypoints[0].transform.rotation) as GameObject;
            speedBooster.transform.position += waypoints[0].transform.right * 10.0f * (Random.insideUnitCircle.x);
            speedBooster.transform.parent = transform;
        }
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

	public Waypoint[] GetWaypoints()
	{
		return waypoints;
	}
}
