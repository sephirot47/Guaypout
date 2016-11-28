using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public ShipPhysicsController shipPhysicsController;
	public float turnSpeed = 1.5f;
    public float tilt = 30f;

	private Rigidbody rb;
	private TrackInformer trackInformer;
    private TrackInformer.TrackInfo info;

	void Start() {
		rb = shipPhysicsController.getRigidBody ();
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

        Waypoint targetBefore = trackInformer.GetClosestPointBefore(transform.position);
        Waypoint targetAfter = trackInformer.GetClosestPointAfter(transform.position);
        Waypoint targetAfterAfter = trackInformer.GetPointAfter(targetAfter);
        if (Vector3.Distance(targetAfter.transform.position, targetAfterAfter.transform.position) < 0.5f)
        {
            targetAfterAfter = trackInformer.GetPointAfter(targetAfter);
        }

        Vector3 direction = targetAfterAfter.transform.position - transform.position;
        direction.y = transform.forward.y;
        direction.Normalize();
        Debug.DrawRay(transform.position, direction, Color.blue, 0.0f, false);

        // Turn
        //Quaternion rotation = Quaternion.LookRotation(direction, info.normal);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        //transform.forward = direction;

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position,  transform.right, out hitInfo, 999.9f, shipPhysicsController.trackLayer))
        {
            rb.AddForce(hitInfo.normal * 3.0f * (1.0f / hitInfo.distance));
        }

        if (Physics.Raycast(transform.position, -transform.right, out hitInfo, 999.9f, shipPhysicsController.trackLayer))
        {
            rb.AddForce(hitInfo.normal * 3.0f * (1.0f / hitInfo.distance));
        }

        float turn = 1.0f - Vector3.Dot(direction, transform.forward);
        turn *= Mathf.Sign( AngleAroundAxis(transform.forward, direction, transform.up) );
        shipPhysicsController.setTurn(turn * 8.0f);

        // Thrust
        shipPhysicsController.setThrust(1f);
        //float gravity = rb.velocity.y;
        //rb.velocity = transform.forward * 50f;
        //rb.velocity = new Vector3(rb.velocity.x, gravity, rb.velocity.z);
    }
}
