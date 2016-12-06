using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemOrbRow : MonoBehaviour 
{
    public GameObject orbPrefab;

    private List<ItemOrb> orbs;

	void Start () 
    {
        orbs = new List<ItemOrb>();

        Transform[] orbPositions = GetComponentsInChildren<Transform>();
        foreach (Transform orbPos in orbPositions)
        {
            GameObject orb = GameObject.Instantiate(orbPrefab, orbPos.position, orbPos.rotation) as GameObject;
            orbs.Add( orb.GetComponent<ItemOrb>() );
        }
	}
	
	void Update () {
	
	}
}
