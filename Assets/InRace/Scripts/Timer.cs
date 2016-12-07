using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float totalTime;
	private float timeLeft;

	void Start () {
		totalTime = 0f;
		timeLeft = 0f;
	}

	void Update () {
		timeLeft -= Time.deltaTime;
	}

	public void Set(float t) {
		totalTime = t;
		timeLeft = t;
	}

	public bool Ended() {
		return timeLeft <= 0;
	}

	public float TimeLeft() {
		return timeLeft;
	}

	public float TotalTime() {
		return totalTime;
	}
}
