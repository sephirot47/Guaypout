using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Projectile projectile;
    public Mine mine;
    public float fireTime = 5f;
    public float shieldTime = 5f;

    private Rigidbody rb;
    
    private bool fireEnabled = false;
    private Timer timer;
	private WeaponTimerBarController bar;

    private bool mineEnabled = false;

    private bool shieldEnabled = false;
    private Timer shieldTimer;
    private ParticleSystem ps;
    private Light light;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        timer = gameObject.AddComponent<Timer>();
        shieldTimer = gameObject.AddComponent<Timer>();

        ps = transform.FindChild("Shield").gameObject.GetComponentInChildren<ParticleSystem>();
        light = transform.FindChild("Shield").gameObject.GetComponentInChildren<Light>();
        DisableShield();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer.Ended()) DisableFire();
        if (shieldTimer.Ended()) DisableShield();
	}

    public void EnableFire()
    {
        fireEnabled = true;
        timer.Set(fireTime);
        if (tag == "Player")
        {
			bar = GameObject.Find ("WeaponTimerBar").GetComponent<WeaponTimerBarController> ();
            bar.SetTimer(timer);
        }
    }

    public void DisableFire()
    {
        fireEnabled = false;
    }

	public bool FireEnabled() {
		return fireEnabled;
	}

    public void FireProjectile()
    {
        if (!fireEnabled) return;

        Vector3 spawn = transform.position + transform.forward * 1.5f;
        Projectile proj = Instantiate(projectile, spawn, Quaternion.identity) as Projectile;
        proj.originShip = gameObject;
        proj.transform.forward = transform.forward;
        proj.GetComponent<Rigidbody>().velocity = rb.velocity;
    }

    public void EnableMine()
    {
        mineEnabled = true;
    }

    public void DisableMine()
    {
        mineEnabled = false;
    }

	public bool MineEnabled() {
		return mineEnabled;
	}

	public void ThrowMine(Vector3 direction)
    {
        if (!mineEnabled) return;

        Vector3 spawn = transform.position + transform.forward * 1.5f;
        Mine m = Instantiate(mine, spawn, Quaternion.identity) as Mine;
        m.originShip = gameObject;
        m.transform.forward = transform.forward;
        m.GetComponent<Rigidbody>().velocity = rb.velocity;
		m.direction = direction;
        mineEnabled = false;
    }

    public void EnableShield()
    {
        shieldEnabled = true;
        shieldTimer.Set(shieldTime);
        ps.gameObject.SetActive(true);
        light.gameObject.SetActive(true);
    }

    public void DisableShield()
    {
        shieldEnabled = false;
        ps.gameObject.SetActive(false);
        light.gameObject.SetActive(false);
    }

    public bool ShieldEnabled()
    {
        return shieldEnabled;
    }
}
