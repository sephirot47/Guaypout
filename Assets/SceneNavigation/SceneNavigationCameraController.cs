using UnityEngine;
using System.Collections;

public class SceneNavigationCameraController : MonoBehaviour 
{
    public float navigationSpeed;

    private INavScene sceneNavigatingTo;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        Transform navPoint = sceneNavigatingTo.GetNavigationPointTransform();

        transform.position = Vector3.Lerp(transform.position, navPoint.position, Time.deltaTime * navigationSpeed);
        transform.rotation = Quaternion.Slerp(transform.rotation, navPoint.rotation, Time.deltaTime * navigationSpeed);
        if (Vector3.Distance(navPoint.position, transform.position) <= 1.0f)
        {
            sceneNavigatingTo.Activate();
        }
	}

    public void NavigateTo(INavScene navScene)
    {
        sceneNavigatingTo = navScene;
        Transform navPoint = navScene.GetNavigationPointTransform();
        //transform.rotation = navPoint.rotation;
    }
}
