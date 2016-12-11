using UnityEngine;
using System.Collections;

public class MinimapIcon : MonoBehaviour 
{
	public GameObject target;

    private float initialYOffset;
	void Start () 
	{
        initialYOffset = transform.position.y - target.transform.position.y;
	}

	void Update () 
	{
        transform.position = target.transform.position + Vector3.up * initialYOffset;
		transform.rotation = Quaternion.LookRotation(Vector3.Cross(target.transform.right, Vector3.up), Vector3.up);
	}
}
