using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemFrameController : MonoBehaviour 
{
    public Image iconImage;
    public Sprite[] itemIcons;
	
    public float minScramblePeriod, maxScramblePeriod;
    private float totalScrambleTime, totalTimeThatHasBeenScrambling = 0.0f;
    private int lastRandomIndex = -1;
    private float timeSinceLastScrambleChange = 0.0f;
    private bool scrambling = false;

    void Start () 
    {
        RemoveItemIcon();
	}
	
	void Update () 
    {
        if (scrambling)
        {
            timeSinceLastScrambleChange += Time.deltaTime;
            totalTimeThatHasBeenScrambling += Time.deltaTime;
            float scramblePeriod = Mathf.Lerp(minScramblePeriod, maxScramblePeriod, totalTimeThatHasBeenScrambling / totalScrambleTime);
            if (timeSinceLastScrambleChange >= scramblePeriod)
            {
                int randomIndex = -1; 
                do
                {
                    randomIndex = Random.Range(0, itemIcons.Length);
                }
                while (randomIndex == lastRandomIndex);
                    
                timeSinceLastScrambleChange = 0.0f;
                iconImage.sprite = itemIcons[randomIndex];
                lastRandomIndex = randomIndex;
            }
        }
	}

    public void StartRandomScramble(float scrambleTime)
    {
        scrambling = true;
        iconImage.enabled = true;
        totalScrambleTime = scrambleTime;
        totalTimeThatHasBeenScrambling = 0.0f;
    }

    public void RemoveItemIcon()
    {
        iconImage.enabled = false;
    }

    public void FinishScrambleAndFixThisWonderfulItem(ItemOrb.OrbType itemType)
    {
        scrambling = false;
        iconImage.sprite = itemIcons[(int) itemType];
    }

    public bool IsScrambling()
    {
        return scrambling;
    }
}
