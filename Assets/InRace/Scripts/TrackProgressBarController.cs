using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrackProgressBarController : MonoBehaviour 
{
	public TrackInformer trackInformer;

    public ShipPhysicsController[] shipsToTrack;
	public Image[] iconsOfShipsToTrack;

	void Start () {
	
	}

	void Update () 
	{
		for (int i = 0; i < shipsToTrack.Length; ++i) 
		{
            GameObject ship = shipsToTrack [i].gameObject;
			Image icon = iconsOfShipsToTrack[i];

            float trackProgress = trackInformer.GetTrackProgress(ship.transform.position);
			icon.rectTransform.anchoredPosition = new Vector2(trackProgress * GetComponent<RectTransform>().rect.width, 0);
		}
	}
}
