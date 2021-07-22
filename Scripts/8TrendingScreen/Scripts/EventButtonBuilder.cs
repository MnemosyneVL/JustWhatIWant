using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventButtonBuilder : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private TextMeshProUGUI eventName = default;
    [SerializeField]
    private Image eventImage = default;
    [SerializeField]
    private Button _eventButton = default;
    // Other Fields
    private EventItem storedEvent = default;
    private int _setID = default;
    public void ConstructEventItem(EventItem eventData, int setId)
    {
        _setID = setId;
        storedEvent = eventData;
        eventName.text = eventData.eventName;
        eventImage.sprite = eventData.coverImage;
        _eventButton.onClick.AddListener(OnClickEventItem);
    }

    public void OnClickEventItem()
    {
        EventViewerManager.Instance.ViewEvent(storedEvent, _setID);
    }
}
