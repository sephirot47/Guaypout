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
    private List<Vector2> classificationRowsPositions = null;
    private List<ClassificationRow> classificationRows = null;
    private string[] positionOrdinals = {"1st", "2nd", "3rd", "4th", "5th", "6th", "7th", "8th"};

	void Start () 
    {
        classificationRows = new List<ClassificationRow>( GetComponentsInChildren<ClassificationRow>() );
        classificationRowsPositions = new List<Vector2>();
        foreach (ClassificationRow cr in classificationRows)
        {
            classificationRowsPositions.Add(cr.GetComponent<RectTransform>().anchoredPosition);
        }
	}

    void MoveRows(bool instantly)
    {
        classificationRows.Sort(
            delegate(ClassificationRow cr1, ClassificationRow cr2)
            {
                return -trackInformer.GetTravelledDistance(cr1.ship.transform.position).CompareTo( 
                        trackInformer.GetTravelledDistance(cr2.ship.transform.position)
                );
            }
        );

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

        positionText.text = positionOrdinals[playerPosition];
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
            MoveRows(false);
        }
	}
}
