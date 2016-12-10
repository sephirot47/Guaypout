using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Projectile projectile;
    public Mine mine;
    public float fireTime = 5f;

    private Rigidbody rb;
    
    private bool fireEnabled = false;
    private Timer timer;
    private FireTimeBar bar;

    private bool mineEnabled = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        timer = gameObject.AddComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timer.Ended()) DisableFire();
	}

    public void EnableFire()
    {
        fireEnabled = true;
        timer.Set(fireTime);
        if (tag == "Player")
        {
            bar = gameObject.AddComponent<FireTimeBar>();
            bar.SetTimer(timer);
        }
    }

    public void DisableFire()
    {
        fireEnabled = false;
        if (bar) Destroy(bar);
    }

	public bool FireEnabled() {
		return fireEnabled;
	}

    public void FireProjectile()
    {
        if (!fireEnabled) return;

        Vector3 spawn = transform.position + transform.forward * 1.5f;
        Projectile proj = Instantiate(projectile, spawn, Quaternion.identity) as Projectile;
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
        m.transform.forward = transform.forward;
        m.GetComponent<Rigidbody>().velocity = rb.velocity;
		m.direction = direction;
        mineEnabled = false;
    }
}
