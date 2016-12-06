using UnityEngine;
using System.Collections;

public class SceneNavigationCameraController : MonoBehaviour 
{
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void NavigateTo(Transform navPoint)
    {
        transform.position = navPoint.position;
        //transform.rotation = navPoint.rotation;
    }
}
