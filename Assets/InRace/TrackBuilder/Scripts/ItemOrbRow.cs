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

                GameObject orb = GameObject.Instantiate(orbPrefab, orbPos.position, orbPos.rotation) as GameObject;
                orb.transform.parent = transform;
                int item = i % 3;
                switch (item) {
                    case 0:
                        orb.GetComponent<ItemOrb>().SetType(ItemOrb.OrbType.FIRE);
                        break;
                    case 1:
                        orb.GetComponent<ItemOrb>().SetType(ItemOrb.OrbType.MINE);
                        break;
                    case 2:
                        orb.GetComponent<ItemOrb>().SetType(ItemOrb.OrbType.SHIELD);
                        break;
                }
                orbs.Add(orb.GetComponent<ItemOrb>());
                ++i;
            }
        }
	}
	
	void Update () {
	
	}
}
