using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackInformer : MonoBehaviour 
{
	public LayerMask trackLayer;
	public int RaysNumber;
	public bool showRays;
	public bool showForwardAndNormal;

	public TrackBuilder trackBuilder;

	public class TrackInfo
	{
		public Vector3 normal = Vector3.zero;
		public Vector3 forward = Vector3.zero;
		public Vector3 groundPoint = Vector3.zero;
		public float distanceToGround = 0.0f;
		public bool overTheTrack = false;
	}

	public TrackInfo GetTrackInfo(Vector3 position, 
		Vector3 right, // Needed to get track forward
		Vector3 up,    // Needed to get distanceToGround
		float rayDistance)
	{
		int hitCount = 0;
		RaycastHit hitInfo;
		TrackInfo info = new TrackInfo();
		foreach (Vector3 dir in GetSphereDirections(RaysNumber))
		{
			Vector3 direction = dir;
			if (Vector3.Dot(direction, -up) < 0)
			{
				direction *= -1.0f; // Always raycast down
			}

			direction.Normalize();
			if (showRays)
			{
				Debug.DrawRay(position, direction * rayDistance, Color.red, 0.0f);
			}

			if (Physics.Raycast(position, direction, out hitInfo, rayDistance, trackLayer.value))
			{
				++hitCount;
				info.normal += hitInfo.normal;
			}
		}

		if (hitCount > 0)
		{
			info.normal /= hitCount;
		}

		// Get Distance to ground
		//Debug.DrawRay(position, -up * rayDistance, Color.white, 0.0f);
		if (Physics.Raycast(position, -up, out hitInfo, rayDistance, trackLayer.value))
		{
			info.overTheTrack = true;
			info.groundPoint = hitInfo.point;
			info.distanceToGround = hitInfo.distance;
		}

		info.forward = Vector3.Cross(right, info.normal);

		if (showForwardAndNormal)
		{
			Debug.DrawRay(position, info.forward * rayDistance, Color.blue, 0.0f, false);
			Debug.DrawRay(position, info.normal * rayDistance, Color.green, 0.0f, false);
		}

		return info;
	}

	private Vector3[] GetSphereDirections(int numDirections)
	{
		var pts = new Vector3[numDirections];
		var inc = Mathf.PI * (3 - Mathf.Sqrt(5));
		var off = 2f / numDirections;

		for (int k = 0; k < numDirections; ++k)
		{
			float y = k * off - 1 + (off / 2);
			float r = Mathf.Sqrt(1 - y * y);
			float phi = k * inc;
			float x = (float)(Mathf.Cos(phi) * r);
			float z = (float)(Mathf.Sin(phi) * r);
			pts[k] = new Vector3(x, y, z);
		}
		return pts;
	}

    // Track progress in the passed position, and forward needed too
    // Returns a value between [0.0, 1.0]
    public float GetTrackProgress(Vector3 position) 
	{
        return GetTravelledDistance(position) / GetTotalTrackDistance();
	}

    public Waypoint GetPointAfter(Waypoint waypoint)
    {
        List<Waypoint> waypoints = trackBuilder.GetWaypointsList();
        for (int i = 0; i < waypoints.Count; ++i)
        {
            if (waypoints[i] == waypoint)
            {
                return (i + 1 < waypoints.Count) ? waypoints[i + 1] : null;
            }
        }
        return null;
    }
    public Waypoint GetClosestPointBefore(Vector3 position) { return GetClosestPointBeforeOrAfter(position, true);  }
    public Waypoint GetClosestPointAfter(Vector3 position)  { return GetClosestPointBeforeOrAfter(position, false); }
    private Waypoint GetClosestPointBeforeOrAfter(Vector3 position, bool before)
    { 
        // Find the two consecutive points such that the ship is between them.
        // Amongst these, take the closest ones (there can be many, take only into
        // those that belong to the track pieces around the player (before, over and after))

        TrackPiece trackPieceBelow  = GetTrackPieceBelow(position);
        TrackPiece trackPieceBefore = GetTrackPieceBefore(trackPieceBelow);
        TrackPiece trackPieceAfter  = GetTrackPieceAfter(trackPieceBelow);

        List<Waypoint> waypoints = trackBuilder.GetWaypointsList();

        Waypoint wpBefore = waypoints[waypoints.Count - 1], wpAfter = wpBefore;
        for (int i = 0; i < waypoints.Count - 1; ++i)
        {
            Waypoint wp1 = waypoints[i], wp2 = waypoints[i + 1];
            bool wp1Before = Vector3.Dot(wp1.GetForward(), (position - wp1.transform.position)) > 0;
            bool wp2After  = Vector3.Dot(wp2.GetForward(), (position - wp2.transform.position)) < 0; 
            if (wp1Before && wp2After)
            {
                if (wp1.GetTrackPiece() == trackPieceBefore || 
                    wp1.GetTrackPiece() == trackPieceBelow  || 
                    wp1.GetTrackPiece() == trackPieceAfter)
                {
                    return before ? wp1 : wp2;
                }
            }
        }
        return waypoints[waypoints.Count - 1];
    }

    // Returns the travelled distance on the track (considering waypoints)
    public float GetTravelledDistance(Vector3 position)
    {
        List<Waypoint> waypoints = trackBuilder.GetWaypointsList();
        float travelledDistance = 0.0f;
        Waypoint closestWaypointBefore = GetClosestPointBefore(position);
        int indexOfClosestWaypointBefore = waypoints.IndexOf(closestWaypointBefore);
        for (int i = 0; i < waypoints.Count - 1; ++i)
        {
            if (i >= indexOfClosestWaypointBefore) break;
            Waypoint wp1 = waypoints[i], wp2 = waypoints[i + 1];
            float dist =  Vector3.Distance(wp1.transform.position, wp2.transform.position);
            travelledDistance += dist;
        } 

        // Add the last bit of distance between the last point behind and position
        travelledDistance += Vector3.Distance(closestWaypointBefore.transform.position, position);
        return travelledDistance;
    }

    public float GetTotalTrackDistance()
    {
        List<Waypoint> waypoints = trackBuilder.GetWaypointsList();
        float totalTrackDistance = 0.0f;
        for (int i = 0; i < waypoints.Count - 1; ++i)
        {
            Waypoint wp1 = waypoints[i], wp2 = waypoints[i + 1];
            float dist =  Vector3.Distance(wp1.transform.position, wp2.transform.position);
            totalTrackDistance += dist;
        } 
        return totalTrackDistance;
    }

    private TrackPiece GetTrackPieceBelow(Vector3 position)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(position, Vector3.down, out hitInfo, 999.9f, trackLayer))
        {
            return hitInfo.collider.gameObject.GetComponentInParent<TrackPiece>();
        }
        return null;
    }
    private TrackPiece GetTrackPieceAfter(TrackPiece tp)
    {
        List<TrackPiece> trackPieces = trackBuilder.GetTrackPieces();
        int i = trackPieces.IndexOf(tp);
        return (i+1 < trackPieces.Count) ? trackPieces[i + 1] : null;
    }
    private TrackPiece GetTrackPieceBefore(TrackPiece tp)
    {
        List<TrackPiece> trackPieces = trackBuilder.GetTrackPieces();
        int i = trackPieces.IndexOf(tp);
        return (i-1 >= 0) ? trackPieces[i - 1] : null;
    }

	private Waypoint GetClosestWaypoint(Vector3 position)
	{
		return trackBuilder.GetClosestWaypoint(position);
	}
}