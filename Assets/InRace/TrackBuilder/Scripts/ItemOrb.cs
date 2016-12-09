using UnityEngine;
using System.Collections;

public class ItemOrb : MonoBehaviour {

	public Behaviour halo;
	public float refillTime = 3f;

	private Timer timer;


    void Start () { 
		timer = gameObject.AddComponent<Timer>();
	}

	void Update () {
		if (timer.Ended())
			Enabled(true);
	}

	void OnTriggerEnter(Collider other) {
        if (other.transform.root.tag == "Player" || other.transform.root.tag == "Enemy")
        {
            GameObject ship = other.transform.root.gameObject;
            WeaponController weaponController = ship.GetComponent<WeaponController>();
            weaponController.EnableFire();
            weaponController.EnableMine();

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
