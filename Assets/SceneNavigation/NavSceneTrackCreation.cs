using UnityEngine;
using System.Collections;

public class NavSceneTrackCreation : INavScene
{
    private GameObject canvas;
    private GameObject worldCanvas;
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
