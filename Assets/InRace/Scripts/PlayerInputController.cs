using UnityEngine;
using System.Collections;

public class PlayerInputController : ShipInputController 
{
    public Projectile projectile;
	public float fireTime = 5f;

	private bool fireEnabled;
	private Timer timer;

    void Start()
    {
        base.Start();
		fireEnabled = false;
		timer = gameObject.AddComponent<Timer>();
    }

	void Update()
    {
        base.Update();
        if (currentState == ShipInputController.State.Moving)
        { 
		    float verticalAxis = Input.GetAxis ("Vertical");
		    float horizontalAxis = Input.GetAxis ("Horizontal");
		    shipPhysicsController.SetThrust (verticalAxis);
		    shipPhysicsController.SetTurn (horizontalAxis);
        }
        else if (currentState == ShipInputController.State.Hit)
        {
        }

		if (fireEnabled && Input.GetKeyDown (KeyCode.F))
			fireProjectile ();

		if (timer.Ended ()) disableFire ();
    }

    public void OnGoalPassed()
    {
        GetComponentInChildren<Animator>().SetTrigger("Boosting");
        this.enabled = false;
    }

	public void enableFire() {
		fireEnabled = true;
		timer.Set (fireTime);
	}

	public void disableFire() {
		fireEnabled = false;
	}

    private void fireProjectile()
    {
		Vector3 spawn = transform.position + transform.forward * 1.5f;
		Projectile proj = Instantiate(projectile, spawn, Quaternion.identity) as Projectile;
		proj.transform.forward = transform.forward;
		proj.GetComponent<Rigidbody>().velocity = rb.velocity;
    }
}
