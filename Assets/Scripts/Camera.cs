using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public GameObject player;
	public float minDistance;
	public float maxDistance;
	private float distance;

	void Update () {
		float endDistance = Mathf.Clamp(player.GetComponent<Rigidbody> ().velocity.magnitude * 0.5f, minDistance, maxDistance);
		distance = Mathf.Lerp (distance, endDistance, Time.deltaTime);

		Vector3 offsetDir = Vector3.Normalize(-player.transform.forward + player.transform.up * 0.5f);
		transform.position = player.transform.position + offsetDir * distance;
		transform.LookAt (player.transform, player.transform.up);
	}
}
