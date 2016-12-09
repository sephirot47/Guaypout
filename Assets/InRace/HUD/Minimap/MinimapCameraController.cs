using UnityEngine;
using System.Collections;

public class MinimapCameraController : MonoBehaviour 
{
	private GameObject player;
	private float initialOffsetY;

	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
		initialOffsetY = transform.position.y + player.transform.position.y;
        player.transform.position = Vector3.zero;
	}

	void Update () 
	{
		transform.position = player.transform.position + Vector3.up * initialOffsetY;
		transform.rotation = Quaternion.LookRotation(-Vector3.up, player.transform.forward);
	}
}
