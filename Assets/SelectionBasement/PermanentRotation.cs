using UnityEngine;
using System.Collections;

public class PermanentRotation : MonoBehaviour {

    public float rotationSpeed;

    private bool rotateEnabled = false;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        if (rotateEnabled)
        {
            transform.rotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.up);
        }
	}

    public void SetRotationEnabled(bool rotationEnabled)
    {
        rotateEnabled = rotationEnabled;
    }
}
