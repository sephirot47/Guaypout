using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

    public Projectile projectile;
    public float fireTime = 5f;

    private Rigidbody rb;
    private bool fireEnabled;
    private Timer timer;
    private FireTimeBar bar;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        fireEnabled = false;
        timer = gameObject.AddComponent<Timer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (fireEnabled && Input.GetKeyDown("space"))
            fireProjectile();
        if (timer.Ended()) disableFire();
	}

    public void enableFire()
    {
        fireEnabled = true;
        timer.Set(fireTime);
        if (tag == "Player")
        {
            bar = gameObject.AddComponent<FireTimeBar>();
            bar.SetTimer(timer);
        }
    }

    public void disableFire()
    {
        fireEnabled = false;
        if (bar) Destroy(bar);
    }

    private void fireProjectile()
    {
        Vector3 spawn = transform.position + transform.forward * 1.5f;
        Projectile proj = Instantiate(projectile, spawn, Quaternion.identity) as Projectile;
        proj.transform.forward = transform.forward;
        proj.GetComponent<Rigidbody>().velocity = rb.velocity;
    }

    private void throwMine()
    {
        Vector3 spawn = transform.position + transform.forward * 1.5f;
        Mine mine = Instantiate(projectile, spawn, Quaternion.identity) as Mine;
    }
}
