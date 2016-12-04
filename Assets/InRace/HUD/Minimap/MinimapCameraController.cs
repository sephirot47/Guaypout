using UnityEngine;
using System.Collections;

public class MinimapCameraController : MonoBehaviour 
{
	public GameObject player;

	private float initialOffsetY;

	void Start () 
	{
		initialOffsetY = transform.position.y + player.transform.position.y;
	}

	void Update () 
	{
		transform.position = player.transform.position + Vector3.up * initialOffsetY;
		transform.rotation = Quaternion.LookRotation(-Vector3.up, player.transform.forward);
	}
}
