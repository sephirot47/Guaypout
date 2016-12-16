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
            int randomBeginIndex = Random.Range(0, waypoints.Length-1);
            Vector3 boosterPos = Vector3.Lerp(waypoints[randomBeginIndex].transform.position,
                                              waypoints[randomBeginIndex+1].transform.position,
                                              Random.value);
            Vector3 normal = waypoints[randomBeginIndex].transform.up + waypoints[randomBeginIndex+1].transform.up;
            normal.Normalize();
            Vector3 forward = waypoints[randomBeginIndex+1].transform.position - waypoints[randomBeginIndex].transform.position;
            forward.Normalize();

            speedBooster = GameObject.Instantiate(
                speedBoosterPrefab, 
                boosterPos + normal * 1.5f,
                Quaternion.LookRotation(forward)) as GameObject;

            speedBooster.transform.position += waypoints[0].transform.right * 10.0f * Random.value;
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
