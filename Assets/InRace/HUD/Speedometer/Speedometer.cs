using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Speedometer : MonoBehaviour 
{
    public float upperSpeedLimit;
    public Image speedometerFill;
    public Text speedText;
    public ShipPhysicsController player;

	void Start () 
    {
	    
	}
	
	void Update () 
    {
        Vector3 planarSpeed = Vector3.ProjectOnPlane(player.GetComponent<Rigidbody>().velocity, player.transform.up);
        speedometerFill.fillAmount = planarSpeed.magnitude / upperSpeedLimit;

        speedText.text = ((int)planarSpeed.magnitude) + " Km/h";
	}
}
