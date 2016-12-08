using UnityEngine;
using System.Collections;

public class BasementDoor : MonoBehaviour 
{
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
        leftDoor.transform.position = leftDoorCloseFinishPoint.position;
        rightDoor.transform.position = rightDoorCloseFinishPoint.position;
	}
	
	void Update () 
    {
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

    void OnTriggerEnter(Collider col)
    {
        currentState = DoorState.Opening;
    }

    void OnTriggerExit(Collider col)
    {
        currentState = DoorState.Closing;
    }
}
