using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public ShipPhysicsController shipPhysicsController;
    public GameObject projectile;

	void Update() 
    {
		float verticalAxis = Input.GetAxis ("Vertical");
		float horizontalAxis = Input.GetAxis ("Horizontal");
		shipPhysicsController.SetThrust (verticalAxis);
		shipPhysicsController.SetTurn (horizontalAxis);

        if (Input.GetKeyDown(KeyCode.F))
            fireProjectile();
    }

    public void OnGoalPassed()
    {
        GetComponentInChildren<Animator>().SetTrigger("Boosting");
        this.enabled = false;
    }

    void fireProjectile()
    {
        Debug.Log("Fire!");
        Projectile proj = Instantiate(
            projectile,
            transform.position + transform.forward * 2f,
            Quaternion.identity
        ) as Projectile;
        proj.player = gameObject;
    }
}