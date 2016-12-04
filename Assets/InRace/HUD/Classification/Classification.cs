using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Classification : MonoBehaviour 
{
    public float rowsMoveSpeed;
    public Text positionText;
    public TrackInformer trackInformer;

    private bool firstFrame = true;
    private Dictionary<GameObject, int> winPositions;
    private List<Vector2> classificationRowsPositions = null;
    private List<ClassificationRow> classificationRows = null;
    private string[] positionOrdinals = {"1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th"};

	void Start () 
    {
        winPositions = new Dictionary<GameObject, int>();
        classificationRows = new List<ClassificationRow>( GetComponentsInChildren<ClassificationRow>() );
        classificationRowsPositions = new List<Vector2>();
        foreach (ClassificationRow cr in classificationRows)
        {
            classificationRowsPositions.Add(cr.GetComponent<RectTransform>().anchoredPosition);
        }
	}

    void MoveRows(bool instantly)
    {
        // Order by where they are in the track
        classificationRows.Sort(
            delegate(ClassificationRow cr1, ClassificationRow cr2)
            {
                return -trackInformer.GetTravelledDistance(cr1.ship.transform.position).CompareTo( 
                        trackInformer.GetTravelledDistance(cr2.ship.transform.position)
                );
            }
        );

        // Move the ships that have already finished the race
        // to their fixed "win/finish" position
        int c = 0;
        for (int i = 0; i < classificationRows.Count; ++i)
        {
            if (c++ >= 1000)
                break;
            
            ClassificationRow cr = classificationRows[i];
            if (winPositions.ContainsKey(cr.ship) && 
                i != winPositions[cr.ship])
            {
                classificationRows.RemoveAt(i); // Move it to its win position
                classificationRows.Insert(winPositions[cr.ship], cr);
                i = 0; // Repeat until all of them are correct!
            }
        }

        // Move the classificationRows in UI accordingly
        int playerPosition = 0;
        for (int i = 0; i < classificationRows.Count; ++i)
        {
            ClassificationRow cr = classificationRows[i];
            if (cr.ship.CompareTag("Player")) { playerPosition = i; }

            Vector2 destPos = classificationRowsPositions[i];
            RectTransform rTrans = cr.GetComponent<RectTransform>();
            if (instantly)
            {
                rTrans.anchoredPosition = destPos;
            }
            else
            {
                rTrans.anchoredPosition = 
                    Vector2.Lerp(rTrans.anchoredPosition, destPos, Time.deltaTime * rowsMoveSpeed);
            }
        }

        positionText.text = positionOrdinals[playerPosition]; // Change the position text
    }

	void Update () 
    {
        if (firstFrame)
        {
            firstFrame = false;
            MoveRows(true);
        }
        else
        {
            // Check if a ship has just won the race, and in that case,
            // save its win position
            foreach (ClassificationRow cr in classificationRows)
            {
                if (cr.ship.GetComponent<ShipPhysicsController>().HasFinishedTheRace() &&
                    !winPositions.ContainsKey(cr.ship))
                {
                    winPositions.Add(cr.ship, winPositions.Count);
                }
            }

            MoveRows(false);
        }
	}
}
