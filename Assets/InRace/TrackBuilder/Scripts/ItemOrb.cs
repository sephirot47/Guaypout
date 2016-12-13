using UnityEngine;
using System;
using System.Collections;

public class ItemOrb : MonoBehaviour {

	public Behaviour halo;
    public Material blue;
    public Material purple;
    public Material orange;

    private OrbType type;

    public float itemScrambleTime;

    private ItemFrameController itemFrameController;

    private GameObject shipThatPickedMe = null;
    private float countdownChrono = 0.0f;
    private bool countdownStarted = false, destroyed = false;
    public enum OrbType
    {
        FIRE   = 0,
        MINE   = 1,
        SHIELD = 2
    };

    void Start () 
    { 
        itemFrameController = GameObject.Find("HUD_InGame/ItemFrame").GetComponent<ItemFrameController>();
        Debug.Log(itemFrameController);
	}

	void Update () 
    {
        if (countdownStarted)
        {
            countdownChrono += Time.deltaTime;
            if (countdownChrono >= itemScrambleTime)
            {
                ActivateOrb();
                Destroy(gameObject);
            }
        }
	}

    void ActivateOrb () // Called when the random time has finished
    {
        shipThatPickedMe.GetComponent<ShipSoundManager>().OnWeaponPicked();
        WeaponController weaponController = shipThatPickedMe.GetComponent<WeaponController>();

        SetType( (OrbType) UnityEngine.Random.Range(0, Enum.GetNames(typeof(OrbType)).Length) );

        switch (type)
        {
            case OrbType.FIRE:
                weaponController.EnableFire();
                break;
            case OrbType.MINE:
                weaponController.EnableMine();
                break;
            case OrbType.SHIELD:
                weaponController.EnableShield();
                break;
        }

        if (shipThatPickedMe.tag == "Player")
        {
            itemFrameController.FinishScrambleAndFixThisWonderfulItem(type);
        }
    }

	void OnTriggerEnter(Collider other) 
    {
        GameObject ship = other.transform.root.gameObject;
        if (!destroyed && (ship.tag == "Player" || ship.tag == "Enemy"))
        {
            GetComponent<AudioSource>().Play();
            countdownStarted = true;
            shipThatPickedMe = ship;
            Enabled(false);

            if (shipThatPickedMe.tag == "Player")
            {
                itemFrameController.StartRandomScramble(itemScrambleTime);
            }
            destroyed = true;
		}
	}

    private void Enabled(bool b) 
    {
        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.gameObject.SetActive(b);
            
		GetComponent<Renderer>().enabled = b;
		halo.enabled = b;
		GetComponent<Collider>().enabled = b;
	}

    public void SetType(OrbType t)
    {
        type = t;
    }
}
