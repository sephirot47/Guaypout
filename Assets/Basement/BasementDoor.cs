using UnityEngine;
using System.Collections;

public class BasementDoor : MonoBehaviour 
{
    public float sensorDistance;
    public float openSpeed;
    public GameObject leftDoor, rightDoor;
    public Transform leftDoorOpenFinishPoint, leftDoorCloseFinishPoint, 
    rightDoorOpenFinishPoint, rightDoorCloseFinishPoint;
    public SceneNavigationCameraController navCamera;

    private enum DoorState
    {
        Opening,
        Closing
    };
    private DoorState currentState;

	void Start () 
    {
        currentState = DoorState.Closing;
	}
	
	void Update () 
    {
        float d = Vector3.Distance(transform.position, navCamera.transform.position);
        if (d <= sensorDistance)
        {
            currentState = (currentState == DoorState.Opening ? DoorState.Closing : DoorState.Opening);
        }
        else
        {
            currentState = DoorState.Closing;
        }

        if (currentState == DoorState.Closing)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position,
                leftDoorCloseFinishPoint.position, openSpeed * Time.deltaTime);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position,
                rightDoorCloseFinishPoint.position, openSpeed * Time.deltaTime);
        }
        else if (currentState == DoorState.Opening)
        {
            leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position,
                leftDoorOpenFinishPoint.position, openSpeed * Time.deltaTime);
            rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position,
                rightDoorOpenFinishPoint.position, openSpeed * Time.deltaTime);
        }
	}
}
