﻿using UnityEngine;
using System.Collections;

public class PlayerInputController : ShipInputController 
{
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

        CheckInput();
    }

    public void OnGoalPassed()
    {
        GetComponentInChildren<Animator>().SetTrigger("Boosting");
        this.enabled = false;
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown("space"))
            weaponController.FireProjectile();
        if (Input.GetKeyDown("space"))
			weaponController.ThrowMine(transform.forward + transform.up);
    }
}
