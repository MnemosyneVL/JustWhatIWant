using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CategoryListItemBuilder : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject _eventItemPrefab = default;
    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI categoryName = default;
    [SerializeField]
    private Transform _categoryContentTransform = default;
    // Other Fields
    private SetOfEvents _setOfEvents = default;
    private CategoryTagItem _categoryTag = default;

    public void ConstructCategoryItem(SetOfEvents events, CategoryTagItem categoryTag)
    {
        _categoryTag = categoryTag;
        _setOfEvents = events;
        categoryName.text = categoryTag.tagName;
        CreateEvents(events);
    }

    public void UpdateEvents(SetOfEvents events)
    {
        ClearEvents();
        CreateEvents(events);
    }

    private void CreateEvents(SetOfEvents events)
    {
        foreach (EventItem eventItem in events.recommendedEvents)
        {
            GameObject newEventItem = Instantiate(_eventItemPrefab, _categoryContentTransform);
            newEventItem.GetComponent<EventButtonBuilder>().ConstructEventItem(eventItem, events._id);
        }
    }

    private void ClearEvents()
    {
        foreach (Transform child in _categoryContentTransform)
        {
            Destroy(child.gameObject);
        }
    }

    public CategoryTagItem GetCategoryTagItem() => _categoryTag;
}
