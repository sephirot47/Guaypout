using UnityEngine;
using System.Collections;

public class FireTimeBar : MonoBehaviour {


	public Vector2 pos = new Vector2(Screen.width/2-100,Screen.height-40);
	public Vector2 size = new Vector2(200,20);
	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;

	private Timer timer;
	private float barDisplay = 0f;

	void Start() {
		progressBarFull = ColorTexture (Color.red);
	}

	void OnGUI() {
		// draw the background:
		GUI.BeginGroup (new Rect (pos.x, pos.y, size.x, size.y));
		GUI.Box (new Rect (0,0, size.x, size.y), progressBarEmpty);

		// draw the filled-in part:
		GUI.BeginGroup (new Rect (0, 0, size.x * barDisplay, size.y));
		GUI.Box (new Rect (0,0, size.x, size.y), progressBarFull);
		GUI.EndGroup ();

		GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () {
		barDisplay = timer.TimeLeft() / timer.TotalTime();
	}
		
	public void SetTimer(Timer t) {
		timer = t;
	}

	private Texture2D ColorTexture(Color c) {
		Texture2D t = new Texture2D (1, 1, TextureFormat.ARGB32, false);
		t.SetPixel (0, 0, c);
		t.wrapMode = TextureWrapMode.Repeat;
		t.Apply ();
		return t;
	}
}
