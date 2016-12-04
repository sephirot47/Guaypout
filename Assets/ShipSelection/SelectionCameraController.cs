using UnityEngine;
using System.Collections;

public class SelectionCameraController : MonoBehaviour 
{
    private Vector3 initialOffset;
    private Vector3 smoothDampCurrentVelocity;

    public int currentShipIndex;
    public float lookAtShipPlatformRotationSpeed;

    public GameObject[] shipPlatforms;

	void Start () 
    {
        initialOffset = transform.position - shipPlatforms[currentShipIndex].transform.position;
	}
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            currentShipIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            currentShipIndex++;
        }

        for (int i = 0; i < shipPlatforms.Length; ++i)
        {
            GameObject sp = shipPlatforms[i];
            sp.GetComponent<PermanentRotation>().SetRotationEnabled(i == currentShipIndex);
        }

        currentShipIndex = (currentShipIndex + shipPlatforms.Length) % shipPlatforms.Length;

        Vector3 destinyPos = shipPlatforms[currentShipIndex].transform.position + initialOffset;
        transform.position = Vector3.SmoothDamp(transform.position, destinyPos, ref smoothDampCurrentVelocity, 0.3f);

        Quaternion destinyRot = Quaternion.LookRotation(shipPlatforms[currentShipIndex].transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, destinyRot, Time.deltaTime * lookAtShipPlatformRotationSpeed);
	}
}
