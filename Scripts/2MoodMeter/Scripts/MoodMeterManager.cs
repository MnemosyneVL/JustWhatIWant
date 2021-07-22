using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoodMeterManager : MonoBehaviour
{
    [Header("Settings")]
    public MoodObj[] moods;
    [SerializeField]
    [Tooltip("first two represent coordinates of bottom left corner, second two represent top right corner")]
    private Vector4 _canvasCorners = new Vector4(-438f, -438f, 438f, 438f);

    [Header("References")]
    [SerializeField]
    private RectTransform _emojiTransform = default;
    [SerializeField]
    private Image _emojiImg = default;
    [SerializeField]
    private Text _moodText = default;
    [SerializeField]
    private Camera _sceneCamera = default;

    private Vector2 _matrixCount;
    private Vector2 _minExtremes;
    private Vector2 _maxExtremes;
    private Vector3 newPos;


    private void Start()
    {
        GenerateMoodMatrix();
    }

    private void GenerateMoodMatrix()
    {
        CreateMatrix();


    }

    private void CreateMatrix()
    {
        Vector4 extremes = FindCoordinateExtremes();
        _minExtremes = new Vector2(extremes.x, extremes.y);
        _maxExtremes = new Vector2(extremes.z, extremes.w);

        float xVar = extremes.x;
        float yVar = extremes.y;

        int xAmount = 1;
        int yAmount = 1;
        foreach (MoodObj mood in moods)
        {
            if (mood.minX > xVar)
            {
                xAmount++;
                xVar = mood.minX;
            }
            if (mood.minY > yVar)
            {
                yAmount++;
                yVar = mood.minY;
            }
        }

        _matrixCount = new Vector2(xAmount, yAmount);

    }

    private Vector4 FindCoordinateExtremes()
    {
        float ymin = moods[0].minY;
        float ymax = moods[0].minY;
        float xmin = moods[0].minX;
        float xmax = moods[0].minX;

        foreach (MoodObj mood in moods)
        {
            if (mood.minY < ymin) ymin = mood.minY;
            if (mood.minY > ymax) ymax = mood.minY;
            if (mood.minX < xmin) xmin = mood.minX;
            if (mood.minX < ymax) xmax = mood.minX;
        }

        return new Vector4(xmin, ymin, xmax, ymax);

    }

    private void Update()
    {
        if(MouseCheck())
        {
            UpdateEmoji();
        }
      
    }
    
    private bool MouseCheck()
    {
        if (Input.GetMouseButton(0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateEmoji()
    {
        newPos = Input.mousePosition;
        _emojiTransform.position = newPos;
        newPos.x -= _sceneCamera.pixelWidth / 2;
        newPos.y -= _sceneCamera.pixelHeight / 2;
        //newPos = new Vector3(Mathf.Clamp(newPos.x, _canvasCorners.x, _canvasCorners.z), Mathf.Clamp(newPos.y, _canvasCorners.y, _canvasCorners.w), 0f);


        MoodObj moodVar = moods[0];//CRUTCH__________________________________!!!!!
        float currentX = _minExtremes.x;
        float currentY = _minExtremes.y;
        List<MoodObj> xArray = new List<MoodObj>();

        foreach (MoodObj mood in moods)
        {
            if(newPos.x>= mood.minX && newPos.x >= currentX)
            {
                if(newPos.y >= mood.minY && newPos.y>= currentY)
                {
                    currentX = mood.minX;
                    currentY = mood.minY;
                    moodVar = mood;
                }
            }
        }

        /*foreach (MoodObj mood in moods)
        {
            if (newPos.x >= mood.minX)
                xArray.Add(mood);
        }
        foreach (MoodObj mood in xArray)
        {
            if (newPos.y >= mood.minY && mood.minY > currentY)
            {
                currentY = mood.minY;
                moodVar = mood;
            }
        }*/

        _emojiImg.sprite = moodVar.moodEmoji;
        _moodText.text = moodVar.moodName;
    }

}

[System.Serializable]
public class MoodObj
{
    public string moodName;
    public Sprite moodEmoji;
    public float minX;
    public float minY;
}