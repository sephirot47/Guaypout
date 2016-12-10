using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	private float totalTime;
	private float timeLeft;
	private bool enabled;

	void Start () {
		totalTime = 0f;
		timeLeft = 0f;
		enabled = false;
	}

	void Update () {
		timeLeft -= Time.deltaTime;
	}

	public void Set(float t) {
		totalTime = t;
		timeLeft = t;
		enabled = true;
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

	public bool Enabled() {
		return enabled;
	}

	public void Disable() {
		enabled = false;
	}
}
