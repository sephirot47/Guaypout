using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShipSelectionController : MonoBehaviour 
{
    public int currentShipIndex;
    public ShipPlatform[] shipPlatforms;

	void Start () 
    {
	
	}
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GoToPreviousShip();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            GoToNextShip();
        }

        for (int i = 0; i < shipPlatforms.Length; ++i)
        {
            ShipPlatform sp = shipPlatforms[i];
            sp.GetComponent<PermanentRotation>().SetRotationEnabled(i == currentShipIndex);
        }
	}

    public void AcceptSelection()
    {
        SceneManager.LoadScene("InRace");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public int GetCurrentShipIndex()
    {
        return currentShipIndex;
    }

    public ShipPlatform GetCurrentShipPlatform()
    {
        return shipPlatforms[currentShipIndex];
    }

    public void GoToPreviousShip()
    {
        currentShipIndex--;
        currentShipIndex = (currentShipIndex + shipPlatforms.Length) % shipPlatforms.Length;
    }

    public void GoToNextShip()
    {
        currentShipIndex++;
        currentShipIndex = (currentShipIndex + shipPlatforms.Length) % shipPlatforms.Length;
    }
}
