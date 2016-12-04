using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShipSelectionController : MonoBehaviour 
{
	void Start () 
    {
	
	}
	
	void Update () 
    {
	
	}

    public void AcceptSelection()
    {
        SceneManager.LoadScene("InRace");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
