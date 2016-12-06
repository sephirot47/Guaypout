using UnityEngine;
using System.Collections;

public class ItemOrb : MonoBehaviour {

	public Behaviour halo;
	public float refillTime = 3f;

	private GameObject player;
	private Timer timer;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
		timer = gameObject.AddComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (timer.Ended())
			Enabled(true);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			player.GetComponent<PlayerInputController> ().enableFire ();

			timer.Set (refillTime);
			Enabled(false);
		}
	}

	private void Enabled(bool b) {
		GetComponent<Renderer>().enabled = b;
		halo.enabled = b;
		GetComponent<Collider>().enabled = b;
	}
}
