using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoalDetector : MonoBehaviour 
{
    private const float maxPositionTextSize = 250.0f;
    private Text positionText;

    private bool playerFinished = false;

	void Start () 
    {
        positionText = GameObject.Find("HUD_InGame/Classification/PositionText").GetComponent<Text>();
        positionText.GetComponent<Animator>().enabled = false;
	}
	
	void Update () 
    {
        if (playerFinished)
        {
            positionText.fontSize = ((int) Mathf.Lerp(positionText.fontSize, maxPositionTextSize, 5.0f * Time.deltaTime) );
        }
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
            playerFinished = true;
            positionText.GetComponent<Animator>().enabled = true;
            //Camera.main.GetComponent<CameraController>().SetMode(CameraController.CameraMode.AfterRace);
        }
    }
}
