using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour 
{
	void Start ()
    {
	
	}
	
	void Update () 
    {
	
	}

    public void GoToPlay()
    {
        SceneManager.LoadScene("ShipSelection");
    }

    public void GoToInstructions()
    {
    }

    public void GoToCredits()
    {
    }
}
