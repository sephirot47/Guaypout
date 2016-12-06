using UnityEngine;
using System.Collections;

public class SceneNavigationController : MonoBehaviour 
{
    private SceneNavigationCameraController navCamera;

    public INavScene mainMenuNS, shipSelectionNS, trackCreationNS, inRaceNS, creditsNS,  instructionsNS;

    private INavScene currentNavScene;

	void Start () 
    {
        navCamera = GameObject.Find("SceneNavigationCamera").GetComponent<SceneNavigationCameraController>();
        mainMenuNS.Deactivate();
        shipSelectionNS.Deactivate();
        //trackCreationNS.Deactivate();
        //inRaceNS.Deactivate();
        //creditsNS.Deactivate();
        //instructionsNS.Deactivate();

        currentNavScene = mainMenuNS;
        GoToMainMenuNS();
	}
	
    public void GoToMainMenuNS()
    {
        CommonNavigationHandlingBefore();
        currentNavScene = mainMenuNS;
        CommonNavigationHandlingAfter();
    }

    public void GoToCreditsNS()
    {
        CommonNavigationHandlingBefore();
        currentNavScene = creditsNS;
        CommonNavigationHandlingAfter();
    }

    public void GoToInstructionsNS()
    {
        CommonNavigationHandlingBefore();
        currentNavScene = instructionsNS;
        CommonNavigationHandlingAfter();
    }

    public void GoToSelectShipNS()
    {
        CommonNavigationHandlingBefore();
        currentNavScene = shipSelectionNS;
        CommonNavigationHandlingAfter();
    }

    public void GoToTrackCreationNS()
    {
        CommonNavigationHandlingBefore();
        currentNavScene = trackCreationNS;
        CommonNavigationHandlingAfter();
    }

    public void GoToInRaceNS()
    {
        CommonNavigationHandlingBefore();
        currentNavScene = inRaceNS;
        CommonNavigationHandlingAfter();
    }

    public void CommonNavigationHandlingBefore()
    {
        currentNavScene.Deactivate();
    }
    public void CommonNavigationHandlingAfter()
    {
        currentNavScene.Activate();
        navCamera.NavigateTo(currentNavScene.GetNavigationPointTransform());
    }
}
