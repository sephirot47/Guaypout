using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	Rigidbody rb;

	public float fwdAcceleration = 100f;
	public float bwdAcceleration = 25f;
	float thrust = 0f;

	public float turnStrength = 10f;
	float turn = 0f;

	public float tilt = 30f;

	public LayerMask trackLayer;
	public float hoverForce = 9f;
	public float hoverHeight = 2f;
	public GameObject[] hoverPoints;
	public TrackInformer trackInformer;

	private GameObject model;

	void Start() {
		rb = GetComponent<Rigidbody> ();
		model = transform.FindChild("PlayerModel").gameObject;
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
			if (Physics.Raycast (ray, out hit, hoverHeight, trackLayer)) {
				float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
				rb.AddForceAtPosition (transform.up * hoverForce * proportionalHeight, hoverPoint.transform.position);
				Debug.DrawRay (ray.origin, ray.direction, Color.green, 0f);
			}
		}

		// Forward
        if (thrust != 0)
        {
            rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);
        }

		// Turn
		if (turn != 0)
        	rb.AddRelativeTorque (transform.up * turn * turnStrength);

		// Lock the forward vector
		TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo (transform.position, transform.right, transform.up, hoverHeight);
		if (trackInfo.overTheTrack) {
            if (trackInfo.forward != Vector3.zero)
            {
                Quaternion endRotation = Quaternion.LookRotation(trackInfo.forward, trackInfo.normal);
                transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, 5 * Time.deltaTime);
            }
		} 
		else { 
			// Falling
			Quaternion endRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
			transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, 0.2f * Time.deltaTime);
		}

		// Tilt
		Quaternion endTilt = Quaternion.AngleAxis (-turn * tilt, transform.forward);
		model.transform.rotation = endTilt * transform.rotation;
	}

	public void ApplyBoost(float boost, Vector3 direction)
	{
		rb.AddForce (direction * boost, ForceMode.Impulse);
	}

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("TrackBoundaries"))
        {
            Vector3 contactNormal = col.contacts[0].normal;
            //Vector3 newForward = Vector3.Cross(contactNormal, Vector3.up);
            Vector3 newForward = Vector3.Reflect(transform.forward, contactNormal);
            if (Vector3.Dot(newForward, transform.forward) < 0) newForward *= -1;
            //Quaternion endFixRotation = Quaternion.LookRotation(newForward, transform.up);
            //rb.velocity = rb.velocity.magnitude * col.contacts[0].normal * 0.5f;
            //transform.rotation = endFixRotation;

            transform.forward = newForward + transform.forward * 2.0f;
            //rb.velocity = Vector3.Reflect(rb.velocity, contactNormal);
        }
    }
}
