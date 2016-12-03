﻿using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public enum CameraMode
    {
        RaceBegin,
        AfterRaceBegin
    };

	public GameObject player;
	public float minDistance;
	public float maxDistance;
	private float distance;

    public float moveSpeed;
    public float rotSpeed;

    private CameraMode currentMode = CameraMode.RaceBegin;

	void FixedUpdate () 
    {
        if (currentMode == CameraMode.AfterRaceBegin)
        {
            float endDistance = Mathf.Clamp(player.GetComponent<Rigidbody>().velocity.magnitude * 0.5f, minDistance, maxDistance);
            distance = Mathf.Lerp(distance, endDistance, Time.deltaTime);

            Vector3 offsetDir = Vector3.Normalize(-player.transform.forward + player.transform.up * 0.25f);
            transform.position = Vector3.Lerp(transform.position, 
                player.transform.position + offsetDir * distance,
                Time.fixedDeltaTime * moveSpeed);

            Vector3 lookPoint = player.transform.position + player.transform.forward * 2.0f;
            Quaternion endRotation = Quaternion.LookRotation(lookPoint - transform.position, player.transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, Time.fixedDeltaTime * rotSpeed);
        }
        else
        {
        }
	}

    public void SetMode(CameraMode mode)
    {
        currentMode = mode;
        if (currentMode == CameraMode.AfterRaceBegin)
        {
            GetComponent<Animator>().enabled = false;
        }
    }
}