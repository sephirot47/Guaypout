using UnityEngine;
using System.Collections;

public class Floating : MonoBehaviour 
{
    public float floatAmount;
    public float floatVelocity;

    private float time;
	void Start () 
    {
        time = Random.Range(0.0f, 3.14592f);
        floatAmount *= Random.Range(0.8f, 1.2f);
        floatVelocity *= Random.Range(0.8f, 1.2f);
	}
	
	void Update ()
    {
        time += floatVelocity * Time.deltaTime;
        transform.position += Vector3.up * (Mathf.Sin(time) * floatAmount);
	}
}
