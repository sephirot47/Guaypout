using UnityEngine;
using System.Collections;

public class SpeedBooster : MonoBehaviour
{
    public float boost;

    public void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player")) // If it's the player
        {
			PlayerController player = col.GetComponent<PlayerController>();
            player.GetComponentInChildren<Animator>().SetTrigger("Boosting");
			player.ApplyBoost(boost, transform.forward);
        }
    }
}
