using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ItemOrbRow : MonoBehaviour 
{
    public GameObject orbPrefab;

    private List<ItemOrb> orbs;

	void Start () 
    {
        if (SceneManager.GetActiveScene().name == "InRace")
        {
            orbs = new List<ItemOrb>();

            Transform[] orbPositions = GetComponentsInChildren<Transform>();
            foreach (Transform orbPos in orbPositions)
            {
                GameObject orb = GameObject.Instantiate(orbPrefab, orbPos.position, orbPos.rotation) as GameObject;
                orb.transform.parent = transform;
                orbs.Add(orb.GetComponent<ItemOrb>());
            }
        }
	}
	
	void Update () {
	
	}
}
