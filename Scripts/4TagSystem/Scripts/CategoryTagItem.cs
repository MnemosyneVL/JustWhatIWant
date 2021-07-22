using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CategoryTagItem
{
    public string tagName = default;
    public CategoryTagItem(string tagName)
    {
        this.tagName = tagName;
    }
}
