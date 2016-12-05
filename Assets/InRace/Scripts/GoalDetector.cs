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
        EnemyInputController enemy = col.GetComponentInParent<EnemyInputController>();
        PlayerInputController player = col.GetComponentInParent<PlayerInputController>();
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
