using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float timeLeft;

	void Start () {
		timeLeft = 0f;
	}

	void Update () {
		timeLeft -= Time.deltaTime;
	}

	public void Set(float t) {
		timeLeft = t;
	}

	public bool Ended() {
		return timeLeft <= 0;
	}
}
