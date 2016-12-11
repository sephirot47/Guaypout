using UnityEngine;
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

            ParticleSystem boostEffect = ship.transform.FindChild("BoostEffect").GetComponentInChildren<ParticleSystem>();
            boostEffect.loop = false;
            boostEffect.time = 0;
            boostEffect.Play();

			ship.ApplyBoost(boost, transform.forward);
        }
    }
}
