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
            int i = 0;
            foreach (Transform orbPos in orbPositions)
            {
                if (orbPos == transform) continue;

                GameObject orb = GameObject.Instantiate(orbPrefab, orbPos.position + Vector3.up, orbPos.rotation) as GameObject;
                //orb.transform.position += Vector3.forward * Random.insideUnitCircle.x * 3.0f;
                orb.transform.parent = transform;
                orbs.Add(orb.GetComponent<ItemOrb>());
                ++i;
            }
        }
	}
	
	void Update () {
	
	}
}
