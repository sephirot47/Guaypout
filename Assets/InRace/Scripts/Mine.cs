using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour {

	public Vector3 direction;

    private Rigidbody rb;
    Timer timer;

    private float lastPositionY;
    private bool canExplode = false;
	private InRaceSoundManager soundManager;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
		rb.AddForce(1000f*direction.normalized);
		soundManager = GameObject.Find ("InRaceSoundPlayer").GetComponent<InRaceSoundManager>();
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
                float radius = 10f;
                Vector3 explosionPos = transform.position;
                Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
                foreach (Collider hit in colliders)
                {
                    if (hit.transform.root.tag == "Player" || hit.transform.root.tag == "Enemy")
                    {
						soundManager.PlayExplosionSound (transform.position);
                        hit.GetComponentInParent<Rigidbody>().AddExplosionForce(power, explosionPos, radius);
                        hit.GetComponentInParent<ShipInputController>().OnHit();
                    }

                }
                Destroy(gameObject);
            }
        }
    }
}
