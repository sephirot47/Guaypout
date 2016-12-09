using UnityEngine;
using System.Collections;

public class NavSceneInstructions : INavScene
{
    private GameObject canvas, worldCanvas;
    new void Awake()
    {
        base.Awake();
        canvas = transform.FindChild("Canvas").gameObject;
        worldCanvas = transform.FindChild("WorldCanvas").gameObject;
    }

    override public void Activate ()
    {
        canvas.SetActive(true);
        worldCanvas.SetActive(true);
    }

    override public void Deactivate ()
    {
        canvas.SetActive(false);
        worldCanvas.SetActive(false);
    }
}
