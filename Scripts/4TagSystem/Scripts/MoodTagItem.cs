using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoodTagItem
{
    public string tagName = default;
    public float weight = default;

    public MoodTagItem(string name, float score)
    {
        tagName = name;
        weight = score;
    }
    public MoodTagItem()
    {
    }
}
