using UnityEngine;
using System.Collections;

public class ItemOrb : MonoBehaviour {

	public Behaviour halo;
	public float refillTime = 3f;

	private GameObject player;
	private Timer timer;


    void Start () {
        player = GameObject.FindGameObjectWithTag("Player"); 
		timer = gameObject.AddComponent<Timer>();
	}

	void Update () {
		if (timer.Ended())
			Enabled(true);
	}

	void OnTriggerEnter(Collider other) {
        if (other.transform.root.tag == "Player") 
        {
            player.GetComponent<PlayerInputController>().enableFire();

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
