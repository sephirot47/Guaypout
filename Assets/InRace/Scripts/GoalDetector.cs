using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalDetector : MonoBehaviour 
{

	void Start () 
    {
	}

    void OnTriggerEnter(Collider col)
    {
        ShipPhysicsController ship = col.GetComponentInParent<ShipPhysicsController>();
        EnemyController enemy = col.GetComponentInParent<EnemyController>();
        PlayerController player = col.GetComponentInParent<PlayerController>();
        if (ship) { ship.OnGoalPassed(); }
        if (enemy) { enemy.OnGoalPassed(); }
        if (player) 
        { 
            player.OnGoalPassed();
            GameObject.Find("GameFlowController").GetComponent<GameFlowController>().
                    SetState(GameFlowController.State.RaceFinished);
        }
    }
}
