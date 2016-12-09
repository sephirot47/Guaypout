using UnityEngine;
using System.Collections;

public class ShipPlatform : MonoBehaviour 
{
    [Range (0,1)]
    [SerializeField]
    private float speed, control, resilience;

    [SerializeField]
    private string shipName;

    [SerializeField]
    private Sprite characterFaceSprite;

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
        return shipName;
    }

    public Sprite GetCharacterFaceSprite()
    {
        return characterFaceSprite;
    }
}
