using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodMeterRequester : MonoBehaviour
{
    [System.Serializable]
    public class MoodMeterRow
    {
        public List<MoodTagItem> moodRow = new List<MoodTagItem>();
    }

    [Header("Settings")]
    [SerializeField]
    public List<MoodMeterRow> moodColumn = new List<MoodMeterRow>();

    //Other Fields
    private float _cellSizeX;
    private float _cellSizeY;


    private void Start()
    {
        CalculateCellSize();
    }

    private void CalculateCellSize()
    {
        _cellSizeY = 1f / moodColumn[0].moodRow.Count;
        _cellSizeX = 1f / moodColumn.Count;
    }

    public List<MoodTagItem> RequestMoodFromMoodMeter(float xCoordinate , float yCoordinate)
    {
        float xPos = xCoordinate / _cellSizeX;
        if (xPos <= 0f) xPos = 0.001f;
        float yPos = yCoordinate / _cellSizeY;
        if (yPos <= 0f) yPos = 0.001f;

        float rowNr = Mathf.Ceil(yPos);
        float columnNr = Mathf.Ceil(xPos);

        float xOffset = 0.5f - ((float)columnNr - xPos);
        float yOffset = 0.5f - ((float)rowNr - yPos);

        float yTopOffset = 0f;
        float yBottomOffset = 0f;
        float xLeftOffset = 0f;
        float xRightOffset = 0f;
        if (xOffset > 0f)
        {
            xRightOffset = xOffset * 2f;
        }
        else
        {
            xLeftOffset = -xOffset * 2f;
        }

        if(yOffset > 0f)
        {
            yTopOffset = yOffset * 2f;
        }
        else
        {
            yBottomOffset = -yOffset * 2f;
        }

        float excess = 0.25f;

        float topScore = excess * yTopOffset;
        float bottomScore = excess * yBottomOffset;
        float leftScore = excess * xLeftOffset;
        float rightScore = excess * xRightOffset;

        float centerScore = (excess - topScore) + (excess - bottomScore) + (excess - leftScore) + (excess - rightScore);

        MoodTagItem centerTag = moodColumn[(int)columnNr-1].moodRow[(int)rowNr-1];
        MoodTagItem topTag = new MoodTagItem();
        MoodTagItem bottomTag = new MoodTagItem();
        MoodTagItem leftTag = new MoodTagItem();
        MoodTagItem rightTag = new MoodTagItem();

        if ((int)rowNr <= moodColumn[0].moodRow.Count - 1)
        {
            topTag = moodColumn[(int)columnNr - 1].moodRow[(int)rowNr];
            topTag.weight = topScore;
        }
        else
        {
            centerScore += topScore;
        }

        if ((int)rowNr - 2 >= 0)
        {
            bottomTag = moodColumn[(int)columnNr - 1].moodRow[(int)rowNr - 2];
            bottomTag.weight = bottomScore;
        }
        else
        {
            centerScore += bottomScore;
        }

        if ((int)columnNr <= moodColumn.Count - 1)
        {
            rightTag = moodColumn[(int)columnNr].moodRow[(int)rowNr - 1];
            rightTag.weight = rightScore;
        }
        else
        {
            centerScore += rightScore;
        }

        if ((int)columnNr - 2 >= 0)
        {
            leftTag = moodColumn[(int)columnNr - 2].moodRow[(int)rowNr - 1];
            leftTag.weight = leftScore;
        }
        else
        {
            centerScore += leftScore;
        }

        centerTag.weight = centerScore;

        List<MoodTagItem> returnList = new List<MoodTagItem>();

        returnList.Add(centerTag);
        if(leftTag != null)
        {
            if(leftTag.weight > 0)
                returnList.Add(leftTag);
        }
        if (rightTag != null)
        {
            if (rightTag.weight > 0)
                returnList.Add(rightTag);
        }
        if (topTag != null)
        {
            if (topTag.weight > 0)
                returnList.Add(topTag);
        }
        if (bottomTag != null)
        {
            if (bottomTag.weight > 0)
                returnList.Add(bottomTag);
        }

        //DEbug

        foreach (var userMood in returnList)
        {
            Debug.Log($"Usermood:{userMood.tagName}, score:{userMood.weight}");
        }
        return returnList;
    }

}
