using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

	public GameObject player;
	private float distance;

	void Start () {
		distance = Vector3.Distance(transform.position, player.transform.position);
	}

	void Update () {
		Vector3 offsetDir = Vector3.Normalize(-player.transform.forward + player.transform.up * 0.5f);
		transform.position = player.transform.position + offsetDir * distance;
		transform.LookAt (player.transform, player.transform.up);
	}
}
