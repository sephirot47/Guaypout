using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShipSelectionController : MonoBehaviour 
{
    public int currentShipIndex;
    public Image characterFaceImage; 
    public ShipPlatform[] shipPlatforms;

    public enum ShipSelection
    {
        Victor,
        Oscar,
        Sanic,
        Cristina
    }
    public static ShipSelection selectedShip;

	void Start () 
    {
        HandleSelection(false);
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
        SceneManager.LoadScene("TrackCreation");
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
        HandleSelection();
    }

    public void GoToNextShip()
    {
        currentShipIndex++;
        currentShipIndex = (currentShipIndex + shipPlatforms.Length) % shipPlatforms.Length;
        HandleSelection();
    }

    public void HandleSelection(bool playSound = true)
    {
        characterFaceImage.sprite = shipPlatforms[currentShipIndex].GetCharacterFaceSprite();
        if (currentShipIndex == 0)
        {
            selectedShip = ShipSelection.Victor;
        }
        else if (currentShipIndex == 1)
        {
            selectedShip = ShipSelection.Oscar;
        }
        else if (currentShipIndex == 2)
        {
            selectedShip = ShipSelection.Sanic;
        }
        else if (currentShipIndex == 3)
        {
            selectedShip = ShipSelection.Cristina;
        }

        if (playSound)
        {
            shipPlatforms[currentShipIndex].GetComponent<AudioSource>().Play();
        }
    }
}
