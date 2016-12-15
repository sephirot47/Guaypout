using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour {

	public AudioClip[] songs;

	private AudioSource source;

	void Start () {
		source = GetComponentInParent<AudioSource> ();
		PlayRandomSong ();
	}

	void Update() {
		if (!source.isPlaying) {
			PlayRandomSong ();
		}
	}

	private void PlayRandomSong() {
		int n = songs.Length;
		int i = Random.Range (0, n);
		source.clip = songs [i];
		source.Play ();
	}
}
