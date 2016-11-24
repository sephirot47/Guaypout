using UnityEngine;
using System.Collections;

public class Waypoint : MonoBehaviour {
    
	void Start () {
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>())
        {
            mr.enabled = false;
        }
	}
	
	void Update () {
	
	}

    Vector3 GetForward()
    {
        return transform.forward;
    }
}
