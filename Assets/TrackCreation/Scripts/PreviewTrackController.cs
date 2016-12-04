using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PreviewTrackController : MonoBehaviour 
{
    public TrackBuilder trackBuilder;
    public int minTrackLongitude, maxTrackLongitude;
    public Slider curvesSlider, slopesSlider, straightSlider, longitudeSlider;

    public static int lastUsedRandomSeed = -1;
    public static float curveProbabilities = 1.0f;
    public static float slopesProbabilities = 1.0f;
    public static float straightProbabilities = 1.0f;
    public static int numPieces = 15;

	void Awake ()
    {
        curvesSlider.value = Random.Range(0.0f, 1.0f);
        slopesSlider.value = Random.Range(0.0f, 1.0f);
        straightSlider.value = Random.Range(0.0f, 1.0f);
        longitudeSlider.value = Random.Range(0.0f, 1.0f);
    }

    void Start()
    {
        UpdateProbabilities();
        GenerateNewTrack();
	}
	
	void Update () 
    {
        UpdateProbabilities();
	}

    private void UpdateProbabilities()
    {
        float[] probs = { curvesSlider.value, slopesSlider.value, straightSlider.value };
        probs = trackBuilder.NormalizedProbabilities(probs);
        curvesSlider.value = probs[0];
        slopesSlider.value = probs[1];
        straightSlider.value = probs[2];

        trackBuilder.piecesProbabilities[0] = trackBuilder.piecesProbabilities[1] = curvesSlider.value;
        trackBuilder.piecesProbabilities[2] = trackBuilder.piecesProbabilities[3] = slopesSlider.value;
        trackBuilder.piecesProbabilities[4] = trackBuilder.piecesProbabilities[5] = 
            trackBuilder.piecesProbabilities[6] = straightSlider.value;
        
        trackBuilder.numPieces = ((int) (longitudeSlider.value  * (maxTrackLongitude - minTrackLongitude))) + minTrackLongitude;
    }

    public void GenerateNewTrack()
    {
        // Save for later use if this is the accepted track
        PreviewTrackController.lastUsedRandomSeed = Random.Range(0, 9999999);
        PreviewTrackController.curveProbabilities = curvesSlider.value;
        PreviewTrackController.slopesProbabilities = slopesSlider.value;
        PreviewTrackController.straightProbabilities = straightSlider.value;
        PreviewTrackController.numPieces = trackBuilder.numPieces;

        trackBuilder.GenerateTrack(lastUsedRandomSeed); // Generate to see the preview
    }

    public void GoToRace()
    {
        SceneManager.LoadScene("InRace");
    }

    public void GoBack()
    {
        SceneManager.LoadScene("ShipSelection");
    }
}
