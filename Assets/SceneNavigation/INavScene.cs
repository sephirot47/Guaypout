using UnityEngine;
using System.Collections;

public class INavScene : MonoBehaviour
{
    protected SceneNavigationCameraController navCamera;

    public void Awake()
    {
        navCamera = GameObject.Find("SceneNavigationCamera").GetComponent<SceneNavigationCameraController>();
    }

    public Transform GetNavigationPointTransform()
    {
        return transform.FindChild("NavPoint");
    }

    virtual public void Activate() {}
    virtual public void Deactivate() {}
}
