using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour {

	public ShipPhysicsController shipPhysicsController;
	public float turnSpeed = 1.5f;
    public float tilt = 30f;
    public float turnSmoothing = 5.0f;
    public float nextWPOrthogonalityTresh = 0.5f;
    public float forwardPredictionDistance = 2.0f;

	private Rigidbody rb;
	private TrackInformer trackInformer;
    private TrackInformer.TrackInfo info;

	void Start() 
    {
        rb = shipPhysicsController.GetRigidbody ();
		trackInformer = GameObject.Find("TrackInformer").GetComponent<TrackInformer>();
	}

	void Update () {
		
	}

    public static float AngleAroundAxis(Vector3 dirA, Vector3 dirB, Vector3 axis)
    {
        dirA = dirA - Vector3.Project(dirA, axis);
        dirB = dirB - Vector3.Project(dirB, axis);
        float angle = Vector3.Angle(dirA, dirB);
        return angle * (Vector3.Dot(axis, Vector3.Cross(dirA, dirB)) < 0 ? -1 : 1);
    }


    void FixedUpdate()
    {
        info = trackInformer.GetTrackInfo(transform.position, transform.right, transform.up, 100f);
        if (!info.overTheTrack) return;

        List<Waypoint> waypointsAfter = trackInformer.GetNPointsAfter(transform.position, 4);

        Vector3 forwardedPosition = (waypointsAfter[2].transform.position + waypointsAfter[3].transform.position) / 2.0f; // transform.position + transform.forward * forwardPredictionDistance;
        forwardedPosition += Vector3.up * 10.0f; // Move up a bit

        Waypoint targetAfterWP = trackInformer.GetPointAfter(forwardedPosition);
        Waypoint targetAfterAfterWP = trackInformer.GetPointAfter(targetAfterWP);
        Vector3 targetAfter = targetAfterWP.transform.position;
        Vector3 targetAfterAfter = targetAfterAfterWP.transform.position;
        if (Vector3.Distance(targetAfter, targetAfterAfter) < 0.5f)
        {
            targetAfterAfterWP = trackInformer.GetPointAfter(targetAfterAfterWP);
            targetAfterAfter = targetAfterAfterWP.transform.position;
        }

        Vector3 v1 = Vector3.ProjectOnPlane(targetAfterAfter - targetAfter, Vector3.up).normalized;
        Vector3 v2 = Vector3.ProjectOnPlane(targetAfter - forwardedPosition, Vector3.up).normalized;
        float nextWPOrthogonality = Vector3.Dot(v1, v2);

        Vector3 target = nextWPOrthogonality < nextWPOrthogonalityTresh ? targetAfterAfter : targetAfter;
        Vector3 direction = target - transform.position;
        direction.y = transform.forward.y;
        direction.Normalize();
        //Debug.DrawLine(transform.position, transform.position + v1 * 5f, Color.red, 0.0f, false);
        //Debug.DrawLine(transform.position, transform.position + v2*5f, Color.green, 0.0f, false);
        Debug.DrawLine(transform.position, target, Color.blue, 0.0f, false);

        // Turn
        //Quaternion rotation = Quaternion.LookRotation(direction, info.normal);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        //transform.forward = direction;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position,  transform.right, out hitInfo, 999.9f, shipPhysicsController.trackLayer))
        {
            rb.AddForce(hitInfo.normal * 8.0f * (1.0f / hitInfo.distance));
        }

        if (Physics.Raycast(transform.position, -transform.right, out hitInfo, 999.9f, shipPhysicsController.trackLayer))
        {
            rb.AddForce(hitInfo.normal * 8.0f * (1.0f / hitInfo.distance));
        }

        float prevTurn = shipPhysicsController.getTurn();
        float s = Mathf.Sign(AngleAroundAxis(transform.forward, direction, transform.up));
        float endTurn = s * (1.0f - Vector3.Dot(direction, transform.forward)) * 15.0f;
        float turn = Mathf.Lerp(prevTurn, endTurn, Time.fixedDeltaTime * turnSmoothing);
        shipPhysicsController.SetTurn(turn);

        // Thrust
        shipPhysicsController.SetThrust(1f);
        //float gravity = rb.velocity.y;
        //rb.velocity = transform.forward * 50f;
        //rb.velocity = new Vector3(rb.velocity.x, gravity, rb.velocity.z);
    }
}
