using UnityEngine;
using System.Collections;

public class NavSceneMainMenu : INavScene
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
    }

    override public void Deactivate ()
    {
        canvas.SetActive(false);
    }
}
