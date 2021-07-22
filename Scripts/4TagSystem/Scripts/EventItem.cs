using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "ScriptableObjects/Events/Create New Event", order = 1)]
public class EventItem : ScriptableObject
{
    public int id = default;
    public string eventName = default;
    public string eventDate = default;
    public string eventLocation = default;
    public string description = default;
    public Sprite coverImage = default;
    public List<MoodTagItem> moodTags = new List<MoodTagItem>();
    public List<CategoryTagItem> categoryTags = new List<CategoryTagItem>();
    public List<string> urls = new List<string>();
}
