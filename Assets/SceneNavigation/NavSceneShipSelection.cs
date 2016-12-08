using UnityEngine;
using System.Collections;

public class NavSceneShipSelection : INavScene 
{
    private GameObject canvas;
    new void Awake()
    {
        base.Awake();
        canvas = transform.FindChild("Canvas").gameObject;
    }

    override public void Activate ()
    {
        canvas.SetActive(true);
        navCamera.GetComponent<SelectionCameraController>().enabled = true;
        navCamera.GetComponent<SceneNavigationCameraController>().enabled = false;
    }

    override public void Deactivate ()
    {
        canvas.SetActive(false);
        navCamera.GetComponent<SelectionCameraController>().enabled = false;
        navCamera.GetComponent<SceneNavigationCameraController>().enabled = true;
    }
}
