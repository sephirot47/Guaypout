using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameFlowController : MonoBehaviour 
{
    public enum State
    {
        Empty,
        RaceBegin,
        CountDown,
        InRace,
        Paused,
        RaceFinished
    };
    private State currentState = State.Empty;
    private State previousState = State.Empty;

    public CameraController camController;
    public Canvas pauseCanvas;

    public float raceBeginTime;
    private float raceBeginChrono, countDownChrono, raceFinishedChrono;

    public Text countDownText, goText;
    private Text positionText;
    private Button replayButton, backToMenuButton;
    private const float maxPositionTextSize = 250.0f;

    void Awake ()
    {
        HandleSelection();
    }

    void Start () 
    {
        raceBeginChrono = countDownChrono = raceFinishedChrono = 0;
        goText.enabled = false;
        positionText = GameObject.Find("HUD_InGame/Classification/PositionText").GetComponent<Text>();

        replayButton = GameObject.Find("HUD_InGame/ReplayButton").GetComponent<Button>();
        replayButton.gameObject.SetActive(false);
        backToMenuButton = GameObject.Find("HUD_InGame/BackToMenuButton").GetComponent<Button>();
        backToMenuButton.gameObject.SetActive(false);

        pauseCanvas.gameObject.SetActive(false);

        positionText.GetComponent<Animator>().enabled = false;
        SetState(State.RaceBegin);
	}
	
	void Update () 
    {
        if (currentState == State.RaceBegin)
        {
            raceBeginChrono += Time.deltaTime;
            if (raceBeginChrono >= raceBeginTime)
            {
                SetState(State.CountDown);
            }
        }
        else if (currentState == State.CountDown)
        {
            countDownChrono += Time.deltaTime;
            if (countDownChrono < 3)
            {
                countDownText.text = (3 - ((int)countDownChrono)).ToString();

                float fract = countDownChrono - ((int)countDownChrono); // Scale animation
                float scale = 1.0f - fract;
                countDownText.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);
            }
            else if (!goText.enabled)
            {
                goText.enabled = true;
                goText.GetComponent<Animator>().SetTrigger("FadeOut");
            }
            else
            {
                SetState(State.InRace);
            }
        }
        else if (currentState == State.InRace)
        {
        }
        else if (currentState == State.RaceFinished)
        {
            positionText.fontSize = ((int) Mathf.Lerp(positionText.fontSize, maxPositionTextSize, 5.0f * Time.deltaTime) );
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            InRaceSoundManager.PlayPauseSound();
            if (currentState == State.Paused)
            {
                UnPause();
            }
            else
            {
                SetState(State.Paused);
            }
        }
	}

    public void SetState(State st)
    {
        if (currentState == st)
            return;
        
        previousState = currentState;
        currentState = st;

        countDownText.enabled = false;
        if (currentState == State.RaceBegin)
        {
            camController.SetMode(CameraController.CameraMode.RaceBegin);
            SetShipControllersEnabled(false);
        }
        else if (currentState == State.CountDown)
        {
            GetComponent<AudioSource>().Play();
            camController.SetMode(CameraController.CameraMode.InRace);
            countDownText.enabled = true;
            SetShipControllersEnabled(false);
        }
        else if (currentState == State.InRace)
        {
            SetShipControllersEnabled(true);
        }
        else if (currentState == State.RaceFinished)
        {
            SetShipControllersEnabled(false);
            replayButton.gameObject.SetActive(true);
            backToMenuButton.gameObject.SetActive(true);
            positionText.GetComponent<Animator>().enabled = true;
            //Camera.main.GetComponent<CameraController>().SetMode(CameraController.CameraMode.AfterRace);
        }

        if (currentState == State.Paused)
        {
            Time.timeScale = 0.0f;
            pauseCanvas.gameObject.SetActive(true);
            SetShipControllersEnabled(false);
        }
        else
        {
            Time.timeScale = 1.0f;
            pauseCanvas.gameObject.SetActive(false);
        }
    }


    public void UnPause()
    {
        SwitchToPreviousState();
    }

    private void SwitchToPreviousState()
    {
        SetState(previousState);
    }

    private void SetShipControllersEnabled(bool enabled)
    {
        foreach (ShipInputController sic in FindObjectsOfType<ShipInputController>())
        {
            sic.enabled = enabled;
        }
    }

    public void Replay()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("InRace");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    private void HandleSelection()
    {
        GameObject victorShip = GameObject.Find("Victor");
        GameObject oscarShip = GameObject.Find("Oscar");
        GameObject sanicShip = GameObject.Find("Sanic");
        GameObject cristinaShip = GameObject.Find("Cristina");

        victorShip.tag = "Enemy"; oscarShip.tag = "Enemy";
        sanicShip.tag = "Enemy"; cristinaShip.tag = "Enemy";
        if (ShipSelectionController.selectedShip == ShipSelectionController.ShipSelection.Victor)
        {
            Destroy(victorShip.GetComponent<EnemyInputController>());
            Destroy(oscarShip.GetComponent<PlayerInputController>());
            Destroy(sanicShip.GetComponent<PlayerInputController>());
            Destroy(cristinaShip.GetComponent<PlayerInputController>());
            victorShip.tag = "Player"; 
        }
        else if (ShipSelectionController.selectedShip == ShipSelectionController.ShipSelection.Oscar)
        {
            Destroy(victorShip.GetComponent<PlayerInputController>());
            Destroy(oscarShip.GetComponent<EnemyInputController>());
            Destroy(sanicShip.GetComponent<PlayerInputController>());
            Destroy(cristinaShip.GetComponent<PlayerInputController>());
            oscarShip.tag = "Player"; 
        }
        else if (ShipSelectionController.selectedShip == ShipSelectionController.ShipSelection.Sanic)
        {
            Destroy(victorShip.GetComponent<PlayerInputController>());
            Destroy(oscarShip.GetComponent<PlayerInputController>());
            Destroy(sanicShip.GetComponent<EnemyInputController>());
            Destroy(cristinaShip.GetComponent<PlayerInputController>());
            sanicShip.tag = "Player"; 
        }
        else if (ShipSelectionController.selectedShip == ShipSelectionController.ShipSelection.Cristina)
        {
            Destroy(victorShip.GetComponent<PlayerInputController>());
            Destroy(oscarShip.GetComponent<PlayerInputController>());
            Destroy(sanicShip.GetComponent<PlayerInputController>());
            Destroy(cristinaShip.GetComponent<EnemyInputController>());
            cristinaShip.tag = "Player"; 
        }
    }
}
