using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Required when Using UI elements.

public class WeaponTimerBarController : MonoBehaviour {

	public Image backgound;
	public Image foreground;

	private bool enabled;
	private Timer timer;

	void Start() {
		SetEnabled (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!enabled) return;

		float amount = timer.TimeLeft() / timer.TotalTime();
		foreground.fillAmount = amount;
		if (timer.Ended()) SetEnabled(false);
	}
		
	public void SetTimer(Timer t) {
		timer = t;
		SetEnabled (true);
	}

	public void SetEnabled(bool b) {
		enabled = b;
		backgound.gameObject.SetActive (b);
		foreground.gameObject.SetActive (b);
	}
}
