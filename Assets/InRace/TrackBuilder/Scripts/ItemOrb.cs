using UnityEngine;
using System.Collections;

public class ItemOrb : MonoBehaviour {

	public Behaviour halo;
    public Material blue;
    public Material purple;
	public float refillTime = 3f;

	private Timer timer;
    private OrbType type;

    public enum OrbType
    {
        FIRE,
        MINE
    };

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
			GetComponent<AudioSource>().Play();
            GameObject ship = other.transform.root.gameObject;
            WeaponController weaponController = ship.GetComponent<WeaponController>();

            switch (type)
            {
                case OrbType.FIRE:
                    weaponController.EnableFire();
                    break;
                case OrbType.MINE:
                    weaponController.EnableMine();
                    break;
            }

			timer.Set (refillTime);
			Enabled(false);
		}
	}

	private void Enabled(bool b) {
		GetComponent<Renderer>().enabled = b;
		halo.enabled = b;
		GetComponent<Collider>().enabled = b;
	}

    public void SetType(OrbType t)
    {
        type = t;
        switch (type)
        {
            case OrbType.FIRE:
                GetComponent<Renderer>().material = blue;
                break;
            case OrbType.MINE:
                GetComponent<Renderer>().material = purple;
                break;
        }
    }
}
