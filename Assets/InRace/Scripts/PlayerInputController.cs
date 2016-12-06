using UnityEngine;
using System.Collections;

public class PlayerInputController : ShipInputController 
{
    public Projectile projectile;

    new void Start()
    {
        base.Start();
    }

    new void Update()
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

		if (Input.GetKeyDown (KeyCode.F))
			fireProjectile ();
    }

    public void OnGoalPassed()
    {
        GetComponentInChildren<Animator>().SetTrigger("Boosting");
        this.enabled = false;
    }

    void fireProjectile()
    {
		Vector3 spawn = transform.position + transform.forward * 1.5f;
		Projectile proj = Instantiate(projectile, spawn, Quaternion.identity) as Projectile;
		proj.transform.forward = transform.forward;
		proj.GetComponent<Rigidbody>().velocity = rb.velocity;
    }
}
