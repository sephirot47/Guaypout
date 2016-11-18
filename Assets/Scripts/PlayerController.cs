using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;

	public float fwdAcceleration = 100f;
	public float bwdAcceleration = 25f;
	float thrust = 0f;

	public float turnStrength = 10f;
	float turn = 0f;

	public LayerMask layerMask;
	public float hoverForce = 9f;
	public float hoverHeight = 2f;
	public GameObject[] hoverPoints;
	public TrackInformer trackInformer;

	void Start() {
		rb = GetComponent<Rigidbody> ();
	}

	void Update() {
		// Main thrust
		float verticalAxis = Input.GetAxis ("Vertical");
		if (verticalAxis > 0)
			thrust = verticalAxis * fwdAcceleration;
		else if (verticalAxis < 0)
			thrust = verticalAxis * bwdAcceleration;

		// Turning
		float horizontalAxis = Input.GetAxis("Horizontal");
		if (horizontalAxis != 0)
			turn = horizontalAxis;
	}

	void FixedUpdate() {
		// Hover force
		RaycastHit hit;
		for (int i = 0; i < hoverPoints.Length; ++i) {
			GameObject hoverPoint = hoverPoints[i];
			Ray ray = new Ray(hoverPoint.transform.position, -transform.up);
			Debug.DrawRay (ray.origin, ray.direction, Color.red, 0f);
			if (Physics.Raycast (ray, out hit, hoverHeight, layerMask)) {
				float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
				rb.AddForceAtPosition (transform.up * hoverForce * proportionalHeight, hoverPoint.transform.position);
				Debug.DrawRay (ray.origin, ray.direction, Color.green, 0f);
			}
		}

		// Forward
		if (thrust != 0)
			rb.AddForce (transform.forward * thrust);

		// Turn
		if (turn != 0)
			rb.AddRelativeTorque (transform.up * turn * turnStrength);

		// Lock the forward vector
		TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo (transform.position, transform.right, transform.up, hoverHeight * 2);
		Quaternion endRotation = Quaternion.LookRotation (trackInfo.forward, trackInfo.normal);
		//transform.rotation = endRotation;
		transform.rotation = Quaternion.Slerp (transform.rotation, endRotation, 5*Time.deltaTime);
	}
}
