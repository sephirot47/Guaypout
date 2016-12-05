using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public enum CameraMode
    {
        RaceBegin,
        InRace,
        AfterRace
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
        // Movement handling
        if (currentMode == CameraMode.InRace || currentMode == CameraMode.AfterRace)
        {
            float endDistance = Mathf.Clamp(player.GetComponent<Rigidbody>().velocity.magnitude * 0.5f, minDistance, maxDistance);
            distance = Mathf.Lerp(distance, endDistance, Time.deltaTime);

            Vector3 offsetDir = Vector3.Normalize(-player.transform.forward + 0.5f*player.transform.up);
            Vector3 newPosition = transform.position = Vector3.Lerp(
                transform.position, 
                player.transform.position + offsetDir * distance,
                Time.fixedDeltaTime * moveSpeed
            );
        }

        // Rotation handling
        if (currentMode == CameraMode.InRace || currentMode == CameraMode.AfterRace)
        {
            Vector3 lookPoint = player.transform.position + player.transform.forward * 2.0f;
            Quaternion endRotation = Quaternion.LookRotation(lookPoint - transform.position, player.transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, Time.fixedDeltaTime * rotSpeed);
        } 
	}

    public void SetMode(CameraMode mode)
    {
        currentMode = mode;
        if (currentMode == CameraMode.InRace)
        {
            GetComponent<Animator>().enabled = false;
        }
    }
}
