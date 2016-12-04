using UnityEngine;
using System.Collections;

public class SelectionCameraController : MonoBehaviour 
{
    private Vector3 initialOffset;
    private Vector3 smoothDampCurrentVelocity;

    public float lookAtShipPlatformRotationSpeed;

    private ShipSelectionController shipSelectionController;

	void Start () 
    {
        shipSelectionController = GameObject.Find("ShipSelectionController").GetComponent<ShipSelectionController>();
        initialOffset = transform.position - shipSelectionController.GetCurrentShipPlatform().transform.position;
	}
	
	void Update () 
    {
        ShipPlatform currentShipPlatform = shipSelectionController.GetCurrentShipPlatform();
        Vector3 destinyPos = currentShipPlatform.transform.position + initialOffset;
        transform.position = Vector3.SmoothDamp(transform.position, destinyPos, ref smoothDampCurrentVelocity, 0.3f);

        Quaternion destinyRot = Quaternion.LookRotation(currentShipPlatform.transform.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, destinyRot, Time.deltaTime * lookAtShipPlatformRotationSpeed);
	}
}
