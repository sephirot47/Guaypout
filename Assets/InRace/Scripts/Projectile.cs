using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
    [HideInInspector]
    public GameObject originShip; 

	public float speed = 20f;

    private Rigidbody rb;
	private float height;
	private TrackInformer trackInformer;

	void Start () 
    {
		rb = GetComponentInChildren<Rigidbody> ();
        rb.velocity += transform.forward * speed;

		trackInformer = GameObject.Find("TrackInformer").GetComponent<TrackInformer>();
		TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo(transform.position, transform.right, transform.up, 100f);
		height = trackInfo.distanceToGround;

        Destroy(gameObject, 10f);
	}

	void Update() 
    {
        if (originShip.name == "Victor")
        {
            GetComponentInChildren<Light>().color = Color.red;
        }
        if (originShip.name == "Oscar")
        {
            GetComponentInChildren<Light>().color = Color.green;
        }
        if (originShip.name == "Cristina")
        {
            GetComponentInChildren<Light>().color = Color.magenta;
        }
        if (originShip.name == "Sanic")
        {
            GetComponentInChildren<Light>().color = Color.blue;
        }

		TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo(transform.position, transform.right, transform.up, 100f);
        if (trackInfo.overTheTrack)
        {
            transform.forward = Vector3.Cross(transform.right, trackInfo.normal);
            transform.position = trackInfo.groundPoint + trackInfo.normal * height;
        }
	}

	void OnTriggerEnter(Collider other) 
    {
        if ( originShip != other.transform.root.gameObject &&
            (other.transform.root.tag == "Player" || other.transform.root.tag == "Enemy")) 
        {
            other.gameObject.GetComponentInParent<ShipInputController>().OnHit(originShip);
			Destroy (gameObject);
		}
	}
}
