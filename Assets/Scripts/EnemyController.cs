using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	public ShipPhysicsController shipPhysicsController;

	void Update () {
		shipPhysicsController.setThrust (1f);
	}
}
