using UnityEngine;
using System.Collections;

public class MainMenuCameraController : MonoBehaviour 
{
    public float rotationSpeed;

	void Start () {
	
	}
	
	void Update () 
    {
        transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.up);
	}
}
