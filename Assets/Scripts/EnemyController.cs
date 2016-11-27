using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public ShipPhysicsController shipPhysicsController;
	public float turnSpeed = 1.5f;

	private Rigidbody rb;
	private TrackInformer trackInformer;

	void Start() {
		rb = shipPhysicsController.getRigidBody ();
		trackInformer = GameObject.Find("TrackInformer").GetComponent<TrackInformer>();
	}

	void Update () {
		TrackInformer.TrackInfo info = trackInformer.GetTrackInfo (transform.position, transform.right, transform.up, 100f);
		if (!info.overTheTrack) return;

		Waypoint target = trackInformer.GetClosestPointAfter (transform.position);

		Vector3 direction = target.transform.position - transform.position;
		direction.y = transform.forward.y;
		Debug.DrawRay(transform.position, direction, Color.blue, 0.0f, false);

		Quaternion rotation = Quaternion.LookRotation(direction, info.normal);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed*Time.deltaTime);

		shipPhysicsController.setThrust (1f);
	}
}
