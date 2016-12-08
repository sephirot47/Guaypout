using UnityEngine;
using System.Collections;

public class SceneNavigationCameraController : MonoBehaviour 
{
    public float navigationSpeed;
    public AudioSource navigationAudioSource;

    private INavScene sceneNavigatingTo;
    private bool firstNavigation = true;

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
        if (firstNavigation)
        {
            firstNavigation = false;
        }
        else
        {
            navigationAudioSource.PlayDelayed(0.1f);
        }

        sceneNavigatingTo = navScene;
        Transform navPoint = navScene.GetNavigationPointTransform();
        //transform.rotation = navPoint.rotation;
    }
}
