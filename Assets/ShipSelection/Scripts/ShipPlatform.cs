using UnityEngine;
using System.Collections;

public class ShipPlatform : MonoBehaviour 
{
    [Range (0,1)]
    [SerializeField]
    private float speed, control, resilience;

    [SerializeField]
    private string name;

	void Start () 
    {
	}
	
	void Update () 
    {
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetControl()
    {
        return control;
    }

    public float GetResilience()
    {
        return resilience;
    }

    public string GetName()
    {
        return name;
    }
}
