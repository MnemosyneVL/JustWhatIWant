using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrendingPageManger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private int amountOfEvents = default;
    [Header("References")]
    [SerializeField]
    private RecommendationManagerMeta _recommendationManager = default;
    [SerializeField]
    private GameObject _eventItemPrefab = default;
    [SerializeField]
    private Transform _contentTransform = default;

    SetOfEvents trendingEvents = new SetOfEvents(); // For now these are random events
    private void Start()
    {

        FillInScrollRect();
    }
    private void FillInScrollRect()
    {
        trendingEvents = _recommendationManager.RequestEvents(amountOfEvents);
        foreach (EventItem eventItem in trendingEvents.recommendedEvents)
        {
            GameObject newEventItem = Instantiate(_eventItemPrefab, _contentTransform);
            newEventItem.GetComponent<EventButtonBuilder>().ConstructEventItem(eventItem, trendingEvents._id);
        }
    }
}
