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
    }

    override public void Deactivate ()
    {
        canvas.SetActive(false);
        navCamera.GetComponent<SelectionCameraController>().enabled = false;
    }
}
