using UnityEngine;
using System.Collections;

public class NavSceneShipSelection : INavScene 
{
    private ShipSelectionController ssc;
    private GameObject canvas;
    new void Awake()
    {
        base.Awake();
        ssc = GameObject.Find("ShipSelectionController").GetComponent<ShipSelectionController>();
        canvas = transform.FindChild("Canvas").gameObject;
    }

    override public void Activate ()
    {
        canvas.SetActive(true);
        ssc.GetCurrentShipPlatform().GetComponent<AudioSource>().Play();
        navCamera.GetComponent<SelectionCameraController>().enabled = true;
        navCamera.GetComponent<SceneNavigationCameraController>().enabled = false;
    }

    override public void Deactivate ()
    {
        canvas.SetActive(false);
        ssc.currentShipIndex = 1;
        navCamera.GetComponent<SelectionCameraController>().enabled = false;
        navCamera.GetComponent<SceneNavigationCameraController>().enabled = true;
    }
}
