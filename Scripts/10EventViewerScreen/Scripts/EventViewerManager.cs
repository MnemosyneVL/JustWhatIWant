using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventViewerManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    RecommendationManagerMeta recommendationManager = default;
    [SerializeField]
    GameObject eventViewerPage = default;

    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI _eventName = default;
    [SerializeField]
    private TextMeshProUGUI _eventDescription = default;
    [SerializeField]
    private TextMeshProUGUI _eventCategoryTags = default;
    [SerializeField]
    private TextMeshProUGUI _eventLocation = default;
    [SerializeField]
    private TextMeshProUGUI _eventTime = default;
    [SerializeField]
    private Image _eventImage = default;
    [SerializeField]
    private Button _buyTicketsButton = default;
    [SerializeField]
    private Button _closeButton = default;

    private static EventViewerManager instance = null;

    // Game Instance Singleton
    public static EventViewerManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        instance = this;
    }

    private void Start()
    {
        _closeButton.onClick.AddListener(DisableViewer);
        EnableViewer(false);
    }

    public void ViewEvent(EventItem eventItem, int setID)
    {
        recommendationManager.OnEventPicked(eventItem, setID);
        EnableViewer(true);
        LoadEventData(eventItem, setID);
    }

    private void EnableViewer(bool status)
    {
        eventViewerPage.SetActive(status);
        NavigationManager.Instance.EnableSwiping(status);
    }

    public void DisableViewer()
    {
        EnableViewer(false);
    }

    private void LoadEventData(EventItem eventItem, int setID)
    {
        _eventName.text = eventItem.eventName;
        _eventDescription.text = eventItem.description;
        _eventImage.sprite = eventItem.coverImage;
        _eventLocation.text = eventItem.eventLocation;
        _eventTime.text = eventItem.eventDate;
        string categoryTags = default;
        for (int i = 0; i < eventItem.categoryTags.Count; i++)
        {
            if (eventItem.categoryTags[i].tagName == "empty") continue;
            categoryTags += $"#{eventItem.categoryTags[i].tagName} ";
        }

        _eventCategoryTags.text = categoryTags;
        _buyTicketsButton.onClick.AddListener(delegate { recommendationManager.OnEventPicked(eventItem, setID);});
    }
}
