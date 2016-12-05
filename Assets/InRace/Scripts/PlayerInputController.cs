using UnityEngine;
using System.Collections;

public class PlayerInputController : ShipInputController 
{
    void Start()
    {
        base.Start();
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
    }

    public void OnGoalPassed()
    {
        GetComponentInChildren<Animator>().SetTrigger("Boosting");
        this.enabled = false;
    }
}
