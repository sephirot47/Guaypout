using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed = 20f;
    private Rigidbody rb;

	void Start () {
		rb = GetComponentInChildren<Rigidbody> ();
        rb.velocity += transform.forward * speed;

        Destroy(gameObject, 10f);
	}

	void OnTriggerEnter(Collider other) {
        if (other.transform.root.tag == "Player" || other.transform.root.tag == "Enemy") {
			other.gameObject.GetComponentInParent<ShipInputController>().OnHit();
			Destroy (gameObject);
		}
	}
}
