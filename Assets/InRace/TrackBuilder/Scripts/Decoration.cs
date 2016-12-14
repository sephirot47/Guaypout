using UnityEngine;
using System.Collections;

public class Decoration : MonoBehaviour 
{
    public float appearingProbs;
	void Start ()
    {
        if (Random.Range(0.0f, 1.0f) > appearingProbs)
        {
            Destroy(gameObject);
        }
    }
	
	void Update () 
    {
	
	}
}
