using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectionStatsController : MonoBehaviour 
{
    public Text nameText;

    public float fillBarSpeed;
    public Image speedStatBar, controlStatBar, resilienceStatBar;

    private ShipSelectionController shipSelectionController;

	void Start ()
    {
        shipSelectionController = GameObject.Find("ShipSelectionController").GetComponent<ShipSelectionController>();
	}
	
	void Update ()
    {
        ShipPlatform currentShipPlatform = shipSelectionController.GetCurrentShipPlatform();

        nameText.text = currentShipPlatform.GetName();

        speedStatBar.fillAmount = Mathf.Lerp(speedStatBar.fillAmount, 
            currentShipPlatform.GetSpeed(), Time.deltaTime * fillBarSpeed);
        
        controlStatBar.fillAmount = Mathf.Lerp(controlStatBar.fillAmount, 
            currentShipPlatform.GetControl(), Time.deltaTime * fillBarSpeed);
        
        resilienceStatBar.fillAmount = Mathf.Lerp(resilienceStatBar.fillAmount, 
            currentShipPlatform.GetResilience(), Time.deltaTime * fillBarSpeed);
	}
}
