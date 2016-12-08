using UnityEngine;
using System.Collections;

public class InRaceSoundManager : MonoBehaviour 
{
    public AudioSource buttonClickSource;
    public AudioSource pauseAudioSource;

	void Start () 
    {
        string prevName = name;
        name = "";
        GameObject oldInstance = GameObject.Find(prevName);
        if (oldInstance != null)
        {
            Destroy(oldInstance);
        }

        name = prevName;
        DontDestroyOnLoad(gameObject);
	}
	
	void Update () 
    {
	
	}

    public void PlayButtonClickSound()
    {
        buttonClickSource.Play();
    }
    public static void PlayPauseSound()
    {
        GameObject.Find("InRaceSoundPlayer").GetComponent<InRaceSoundManager>().pauseAudioSource.Play();
    }

    public void AutoDestroy()
    {
        Destroy(gameObject, 3);
    }
}
