using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    [HideInInspector]
    public GameObject player;

    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponentInChildren<Rigidbody>();
        rb.velocity = player.transform.forward * 20f;

        Destroy(gameObject, 10f);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
