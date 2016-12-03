using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public ShipPhysicsController shipPhysicsController;

	void Update() 
    {
		float verticalAxis = Input.GetAxis ("Vertical");
		float horizontalAxis = Input.GetAxis ("Horizontal");
		shipPhysicsController.SetThrust (verticalAxis);
		shipPhysicsController.SetTurn (horizontalAxis);
	}
}
