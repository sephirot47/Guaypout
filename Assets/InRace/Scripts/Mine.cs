using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Vector3 direction = (transform.forward + transform.up).normalized;
        rb.AddForce(direction);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
