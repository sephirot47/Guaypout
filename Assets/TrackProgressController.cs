using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrackProgressController : MonoBehaviour 
{
	public TrackInformer trackInformer;
	public Image trackProgressFillImage;

	public GameObject[] shipsToTrack;
	public Image[] iconsOfShipsToTrack;

	void Start () {
	
	}

	void Update () 
	{
		for (int i = 0; i < shipsToTrack.Length; ++i) 
		{
			GameObject ship = shipsToTrack [i];
			Image icon = iconsOfShipsToTrack[i];

			float trackProgress = trackInformer.GetTrackProgress(ship.transform.position);
			icon.rectTransform.anchoredPosition = new Vector2(trackProgress * GetComponent<RectTransform>().rect.width, 0);
		}
	}
}
