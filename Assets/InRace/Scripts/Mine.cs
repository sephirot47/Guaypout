using UnityEngine;
using System.Collections;

public class Mine : MonoBehaviour 
{
    [HideInInspector]
    public GameObject originShip;

	public Vector3 direction;

    private Rigidbody rb;

    private bool exploded = false, canExplode = false;
	private InRaceSoundManager soundManager;

    Light beepLight;
    private float timeSinceCreated = 0.0f;
	void Start () 
    {
        beepLight = GetComponentInChildren<Light>();
        rb = GetComponent<Rigidbody>();
		rb.AddForce(1000f*direction.normalized);
		soundManager = GameObject.Find ("InRaceSoundPlayer").GetComponent<InRaceSoundManager>();
	}
	
	void Update ()
    {
        timeSinceCreated += Time.deltaTime;
        beepLight.enabled = (timeSinceCreated % 0.4f > 0.2f);

        canExplode = canExplode || (timeSinceCreated >= 0.5f);
	}

    void OnTriggerEnter(Collider other)
    {
        if (canExplode && !exploded)
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
                        hit.GetComponentInParent<ShipInputController>().OnHit(originShip);
                    }

                }

                exploded = true;
                foreach (MeshRenderer mr in  GetComponentsInChildren<MeshRenderer>())
                {
                    mr.enabled = false;
                }
                GetComponentInChildren<ParticleSystem>().Stop();
                GetComponentInChildren<ParticleSystem>().Play();
                Destroy(gameObject, 1.0f);
            }
        }
    }
}
