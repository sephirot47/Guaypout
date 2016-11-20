using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackInformer : MonoBehaviour 
{
	public LayerMask trackLayer;
	public int RaysNumber;
	public bool showRays;
	public bool showForwardAndNormal;

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
}