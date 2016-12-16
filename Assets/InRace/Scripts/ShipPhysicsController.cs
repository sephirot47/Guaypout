using UnityEngine;
using System.Collections;

public class ShipPhysicsController : MonoBehaviour {

	private Rigidbody rb;

	public float fwdAcceleration = 65f;
	public float bwdAcceleration = 30f;
	private float thrust = 0f;

	public float turnStrength = 25f;
	private float turn = 0f;

    public float tilt = 30f;
    public float tiltSmooth;

    public LayerMask trackLayer;
    public LayerMask trackBoundaryLayer;
	public float hoverForce = 40f;
	public float hoverHeight = 5f;
	public GameObject[] hoverPoints;

    private bool hasFinishedTheRace = false;
	private TrackInformer trackInformer;
	private GameObject model;

	void Start() 
    {
		rb = GetComponent<Rigidbody> ();
        model = transform.FindChild("ModelWrapper/ModelTilter").gameObject;
		trackInformer = GameObject.Find("TrackInformer").GetComponent<TrackInformer>();
	}
	
	void FixedUpdate() 
    {
        RaycastHit hit;
        if (Physics.Raycast (transform.position, Vector3.down, out hit, 999.9f, trackLayer))
        {
            Vector3 fixedPositionUpwards = 
                new Vector3(
                    transform.position.x, 
                    Mathf.Max(hit.point.y + 1.0f, transform.position.y), 
                    transform.position.z);
            transform.position = fixedPositionUpwards;
        }

		// Hover force
		for (int i = 0; i < hoverPoints.Length; ++i) 
        {
			GameObject hoverPoint = hoverPoints[i];
			Ray ray = new Ray(hoverPoint.transform.position, -transform.up);
			//Debug.DrawRay (ray.origin, ray.direction, Color.red, 0f);
			if (Physics.Raycast (ray, out hit, hoverHeight, trackLayer)) 
            { 
				float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
				rb.AddForceAtPosition (transform.up * hoverForce * proportionalHeight, hoverPoint.transform.position);
				//Debug.DrawRay (ray.origin, ray.direction, Color.green, 0f);
			}
		}

        // Forward
        if (thrust != 0)
            rb.AddForce(transform.forward * thrust, ForceMode.Acceleration);

        // Turn
        if (turn != 0)
            rb.AddRelativeTorque(transform.up * turn * turnStrength);

        // Correction of track boundaries with forces
        if (GetComponent<ShipInputController>().enabled)
        {
            TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo(transform.position, transform.right, transform.up, hoverHeight);
            if (trackInfo.overTheTrack)
            {
                Waypoint wpBefore = trackInformer.GetNPointsBefore(transform.position, 1)[0];
                Waypoint wpAfter = trackInformer.GetNPointsAfter(transform.position, 1)[0];
                Vector3 trackForward = (wpAfter.transform.position - wpBefore.transform.position).normalized;
                Vector3 trackUp = trackInfo.overTheTrack ? trackInfo.normal : transform.up;

                Vector3 trackRight = Vector3.Cross(trackForward, trackUp);
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, trackRight, out hitInfo, 999.9f, trackBoundaryLayer))
                {
                    float distFactor = (1.0f / hitInfo.distance);
                    rb.AddForce(hitInfo.normal * 400.0f * Mathf.Pow(distFactor, 5.0f));
                }
                if (Physics.Raycast(transform.position, -trackRight, out hitInfo, 999.9f, trackBoundaryLayer))
                {
                    float distFactor = (1.0f / hitInfo.distance);
                    rb.AddForce(hitInfo.normal * 400.0f * Mathf.Pow(distFactor, 5.0f));
                }
            }
        }
        //

        if (hasFinishedTheRace)
        {
            thrust *= 0.97f;
            //rb.AddForce(-transform.forward * 5.0f); // Brake
        }
	}

    void Update()
    {
        // Lock the forward vector
        TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo (transform.position, transform.right, transform.up, hoverHeight);
        if (trackInfo.overTheTrack) {
            if (trackInfo.forward != Vector3.zero)
            {
                Quaternion endRotation = Quaternion.LookRotation(trackInfo.forward, trackInfo.normal);
                rb.rotation = Quaternion.Slerp(rb.rotation, endRotation, 5.0f * Time.deltaTime);
            }
        } 
        else { 
            // Falling
            Quaternion endRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * rb.rotation;
            rb.rotation = Quaternion.Slerp(rb.rotation, endRotation, 0.2f * Time.deltaTime);
        }

        // Tilt
        Quaternion endTilt = Quaternion.AngleAxis (turn * -tilt, Vector3.forward);
        model.transform.localRotation = Quaternion.Slerp(model.transform.localRotation, endTilt, Time.deltaTime * tiltSmooth);
    }

	public void ApplyBoost(float boost, Vector3 direction)
	{
		rb.AddForce (direction * boost, ForceMode.Impulse);
	}

    public void SetThrust(float verticalAxis) 
    {
		// Main thrust
        if (verticalAxis > 0)
            thrust = verticalAxis * fwdAcceleration;
        else if (verticalAxis < 0)
            thrust = verticalAxis * bwdAcceleration;
        else thrust = 0.0f;
	}

	public void SetTurn(float horizontalAxis) 
    {
		// Turning
        turn = horizontalAxis;
	}

	public Rigidbody GetRigidbody() 
    {
        return GetComponent<Rigidbody> ();
	}

    public float GetTurn()
    {
        return turn;
    }
    public float GetThrust()
    {
        return thrust;
    }

    public void OnGoalPassed()
    {
        hasFinishedTheRace = true;
    }

    public bool HasFinishedTheRace()
    {
        return hasFinishedTheRace;
    }
}
