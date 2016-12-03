using UnityEngine;
using UnityEngine.UI;
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

	void Start () 
    {
        raceBeginChrono = countDownChrono = raceFinishedChrono = 0;
        goText.enabled = false;
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
            camera.GetComponent<CameraController>().SetMode(CameraController.CameraMode.AfterRaceBegin);
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
}
