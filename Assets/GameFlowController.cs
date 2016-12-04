using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameFlowController : MonoBehaviour 
{
    public enum State
    {
        RaceBegin,
        CountDown,
        InRace,
        RaceFinished
    };

    public CameraController camera;

    private State currentState;
    public float raceBeginTime;
    private float raceBeginChrono, countDownChrono, raceFinishedChrono;

    public Text countDownText, goText;
    private Text positionText;
    private Button backToMenuButton;
    private const float maxPositionTextSize = 250.0f;

	void Start () 
    {
        raceBeginChrono = countDownChrono = raceFinishedChrono = 0;
        goText.enabled = false;
        positionText = GameObject.Find("HUD_InGame/Classification/PositionText").GetComponent<Text>();
        backToMenuButton = GameObject.Find("HUD_InGame/BackToMenuButton").GetComponent<Button>();
        backToMenuButton.gameObject.SetActive(false);
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
	}

    public void SetState(State st)
    {
        currentState = st;
        countDownText.enabled = false;
        if (currentState == State.RaceBegin)
        {
            camera.GetComponent<CameraController>().SetMode(CameraController.CameraMode.RaceBegin);
            SetShipControllersEnabled(false);
        }
        else if (currentState == State.CountDown)
        {
            camera.GetComponent<CameraController>().SetMode(CameraController.CameraMode.InRace);
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
            backToMenuButton.gameObject.SetActive(true);
            positionText.GetComponent<Animator>().enabled = true;
            //Camera.main.GetComponent<CameraController>().SetMode(CameraController.CameraMode.AfterRace);
        }
    }

    private void SetShipControllersEnabled(bool enabled)
    {
        foreach (EnemyController ec in FindObjectsOfType<EnemyController>())
        {
            ec.enabled = enabled;
        }
        foreach (PlayerController pc in FindObjectsOfType<PlayerController>())
        {
            pc.enabled = enabled;
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
