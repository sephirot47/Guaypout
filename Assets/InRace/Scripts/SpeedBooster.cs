﻿using UnityEngine;
using System.Collections;

public class SpeedBooster : MonoBehaviour
{
    public float boost;

    public void OnTriggerEnter(Collider col)
    {
		if (col.GetComponentInParent<ShipPhysicsController>()) // If it's a ship
        {
            ShipPhysicsController ship = col.GetComponentInParent<ShipPhysicsController>();
            GetComponent<AudioSource>().Play();
            ship.GetComponentInParent<ShipSoundManager>().OnBoost();
			ship.GetComponentInChildren<Animator>().SetTrigger("Boosting");
			ship.ApplyBoost(boost, transform.forward);
        }
    }
}
