using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public enum CameraMode
    {
        RaceBegin,
        PositionBehindPlayer,
        InRace,
        AfterRace
    };

	public float minDistance;
	public float maxDistance;
	private float distance;

    public float moveSpeed;
    public float rotSpeed;

    private GameObject player;
    private bool inFirstPerson = false;
    public CameraMode currentMode = CameraMode.RaceBegin;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // There should be only one ship like this
        distance = Vector3.Distance(transform.position, player.transform.position);
    }

    void Update ()
    {
        // FP / no-FP handling
        if (Input.GetKeyDown(KeyCode.C))
        {
            inFirstPerson = !inFirstPerson;
        }

        if (inFirstPerson)
        {
            transform.position = player.transform.FindChild("FPCameraPosition").position;
        }
    }

	void FixedUpdate () 
    {
        // Camera movement handling
        if (!inFirstPerson) // Third person
        {
            float endDistance = Mathf.Clamp(player.GetComponent<Rigidbody>().velocity.magnitude * 0.5f, minDistance, maxDistance);
            distance = Mathf.Lerp(distance, endDistance, Time.deltaTime);

            Vector3 offsetDir = Vector3.Normalize(-player.transform.forward + 0.5f * player.transform.up);
            Vector3 newPosition = player.transform.position + offsetDir * distance;
            if (currentMode == CameraMode.PositionBehindPlayer)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * 3.0f);
            }
            else if (currentMode != CameraMode.RaceBegin)
            {
                transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * moveSpeed);
            }
        }
        else //First person
        {
            if (currentMode == CameraMode.InRace || currentMode == CameraMode.AfterRace)
            {
                transform.position = player.transform.FindChild("FPCameraPosition").position;
            }
        }

        // Rotation handling
        if (!inFirstPerson) // Third person
        {
            if (currentMode == CameraMode.InRace || currentMode == CameraMode.AfterRace || currentMode == CameraMode.PositionBehindPlayer)
            {
                Vector3 lookPoint = player.transform.position + player.transform.forward * 2.0f;
                Quaternion endRotation = Quaternion.LookRotation(lookPoint - transform.position, player.transform.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, Time.fixedDeltaTime * rotSpeed);
            } 
        }
        else //First person
        {
            if (currentMode == CameraMode.InRace || currentMode == CameraMode.AfterRace)
            {
                Vector3 lookPoint = player.transform.position + player.transform.forward * 99.9f;
                transform.LookAt(lookPoint, Vector3.up);
            } 
        }
	}

    public void SetMode(CameraMode mode)
    {
        currentMode = mode;
        if (currentMode == CameraMode.InRace || currentMode == CameraMode.PositionBehindPlayer)
        {
            GetComponent<Animator>().enabled = false;
        }
    }
}
