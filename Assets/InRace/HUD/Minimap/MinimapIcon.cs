using UnityEngine;
using System.Collections;

public class MinimapIcon : MonoBehaviour 
{
	public GameObject target;

	void Start () 
	{
	
	}

	void Update () 
	{
		transform.position = target.transform.position + Vector3.up * 200.0f;
		transform.rotation = Quaternion.LookRotation(Vector3.Cross(target.transform.right, Vector3.up), Vector3.up);
	}
}
