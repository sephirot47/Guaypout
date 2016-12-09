using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

    private Rigidbody rb;
    Timer timer;

    private float lastPositionY;
    private bool canExplode = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        Vector3 direction = (transform.forward + transform.up).normalized;
        rb.AddForce(1000f*direction);
	}
	
	// Update is called once per frame
	void Update () {
        canExplode = canExplode || (transform.position.y < lastPositionY);
        lastPositionY = transform.position.y;
	}

    void OnTriggerEnter(Collider other)
    {
        if (canExplode)
        {
            if (other.transform.root.tag == "Player" || other.transform.root.tag == "Enemy")
            {
                float power = 5000f;
                float radius = 20f;
                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders)
                {
                    if (hit.transform.root.tag == "Player" || hit.transform.root.tag == "Enemy")
                    {
                        hit.GetComponentInParent<Rigidbody>().AddExplosionForce(power, explosionPos, radius);
                        hit.GetComponentInParent<ShipInputController>().OnHit();
                    }

                }
                Destroy(gameObject);
            }
        }
    }
}
