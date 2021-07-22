using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CategoryFilteringManager
{
    public List<EventItem> FilterSetOfCategoryEvents (List<EventItem> sourceEvents, CategoryTagItem[] selectedTags, int numOfEvents)
    {
        List<EventItem> selectedItems = sourceEvents;
        foreach (CategoryTagItem tag in selectedTags)
        {
            selectedItems = selectedItems.Where(p => p.categoryTags.Any(n => n.tagName == tag.tagName)).ToList();
        }
        selectedItems = CropEventsList(selectedItems, numOfEvents);
        return selectedItems;
    }
    public List<EventItem> CropEventsList(List<EventItem> selectedEvents, int numOfRecommendations)
    {
        if (numOfRecommendations == 0)
            return selectedEvents;
        else
        {
            if (selectedEvents.Count >= numOfRecommendations)
            {
                selectedEvents = selectedEvents.Take(numOfRecommendations).ToList();
                return selectedEvents;
            }
            else return selectedEvents;
        }
    }
}
