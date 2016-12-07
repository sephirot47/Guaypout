using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RandomChars : MonoBehaviour 
{
    public string possibleChars = "abcdefghijklmnopqrstuvwxyz1234567890#-.";
    public float changeSpeed;
    public int minDigits, maxDigits;

    private Text text;
    private float changeChrono = 0.0f;
	void Start () 
    {
        changeSpeed *= Random.Range(0.8f, 1.2f);

        text = GetComponent<Text>();
        FillTextWithRandomPuke();
	}
	
	void Update () 
    {
        changeChrono += Time.deltaTime;
        if (changeChrono >= changeSpeed)
        {
            changeChrono = 0.0f;
            FillTextWithRandomPuke();
        }
	}

    void FillTextWithRandomPuke()
    {
        int digits = Random.Range(minDigits, maxDigits + 1);
        string newRandomString = "";
        for (int i = 0; i < digits; ++i)
        {
            newRandomString += possibleChars[Random.Range(0,possibleChars.Length)];
        }

        text.text = newRandomString;
    }
}
