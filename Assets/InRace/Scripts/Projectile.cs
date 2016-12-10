﻿using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float speed = 20f;

    private Rigidbody rb;
	private float height;
	private TrackInformer trackInformer;

	void Start () {
		rb = GetComponentInChildren<Rigidbody> ();
        rb.velocity += transform.forward * speed;

		trackInformer = GameObject.Find("TrackInformer").GetComponent<TrackInformer>();
		TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo(transform.position, transform.right, transform.up, 100f);
		height = trackInfo.distanceToGround;

        Destroy(gameObject, 10f);
	}

	void Update() {
		TrackInformer.TrackInfo trackInfo = trackInformer.GetTrackInfo(transform.position, transform.right, transform.up, 100f);
		transform.forward = Vector3.Cross (transform.right,trackInfo.normal);
		transform.position = trackInfo.groundPoint + trackInfo.normal * height;
		Debug.DrawRay(transform.position, transform.forward, Color.green, 0);
	}

	void OnTriggerEnter(Collider other) {
        if (other.transform.root.tag == "Player" || other.transform.root.tag == "Enemy") {
			other.gameObject.GetComponentInParent<ShipInputController>().OnHit();
			Destroy (gameObject);
		}
	}
}
