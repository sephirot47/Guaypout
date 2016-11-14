using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public LayerMask trackLayer;

	public float trackRotationSpeed;
	public float maxSpeed;
	private float speed;
	public float acceleration;
	public float tilt;

	public float height;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		Quaternion rotationEnd = Quaternion.identity;
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = transform.forward * moveVertical + transform.right * moveHorizontal;
		speed = Mathf.Min(speed + acceleration, maxSpeed);
		rb.velocity = movement * speed;

		//rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);

		RaycastHit hitInfo;
		Vector3 rayOrigin = transform.position + transform.forward;
		if (Physics.Raycast (rayOrigin, -transform.up, out hitInfo, 999.9f, trackLayer.value)) {
			Vector3 lookDir = Vector3.Cross(transform.right, hitInfo.normal);
			rotationEnd = Quaternion.LookRotation(lookDir, hitInfo.normal);
			Vector3 newPosition = transform.position;
			newPosition.y = (hitInfo.point + hitInfo.normal * height).y;
			rb.MovePosition (newPosition);
		}

		transform.rotation = Quaternion.Slerp (transform.rotation, rotationEnd, trackRotationSpeed * Time.deltaTime);
		Debug.DrawLine (rayOrigin, transform.position - transform.up * 999.9f, Color.green, 0.05f, true);
	}
}
