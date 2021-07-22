using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoodFilteringManager
{
    private int _topScorePercentage = 50;
    private int _secondaryScorePercentage = 30;
    public MoodFilteringManager(int topScorePercentage, int secondaryScorePercentage)
    {
        _topScorePercentage = topScorePercentage;
        _secondaryScorePercentage = secondaryScorePercentage;
    }
    #region Different types of recommendations

    public List<EventItem> RequestMoodEvents(List<EventItem> sourceEvents, List<MoodTagItem> selectedTags, int amountOfEvents)
    {
        Vector3 amounts = CalculateEventsAmount(amountOfEvents, _topScorePercentage, _secondaryScorePercentage);
        List<EventItem> returnEvents = FilterSetOfMoodEvents(sourceEvents, selectedTags.ToArray(), (int)amounts.x, (int)amounts.y, (int)amounts.z);
        return returnEvents;
    }

    private Vector3 CalculateEventsAmount(int totalAmount, int topScorePercentage, int secondaryScorePercentage)
    {
        Vector3 amounts = new Vector3();
        amounts.x = (int)Mathf.Ceil(totalAmount * ((float)topScorePercentage / 100f));
        amounts.y = (int)Mathf.Floor(totalAmount * ((float)secondaryScorePercentage / 100f));
        amounts.z = (int)Mathf.Floor(totalAmount * ((100f - (float)topScorePercentage - (float)secondaryScorePercentage) / 100f));

        return amounts;
    }

    /// <summary>
    /// This method is used to create a list of recommendations based on requested mood tags. The list of recommendations would contain of most fitting events, less fitting events and random events
    /// </summary>
    /// <param name="sourceEvents">list of events that need to be sorted</param>
    /// <param name="selectedTags">mood tags array</param>
    /// <param name="numOfTopScoreEvents">amount of events with highest compatibility score</param>
    /// <param name="numOfSameTagEvents">amount of events with the same mood tags, without repititions with top events</param>
    /// <param name="numOfRandomEvents">amount of random events from the list</param>
    /// <returns>The method returns a list with top events being the first ones, secondary coming next and random being the last. 
    /// If the system can not find requested amount of top and secondary events they will be replaced with random events</returns>
    public List<EventItem> FilterSetOfMoodEvents(List<EventItem> sourceEvents, MoodTagItem[] selectedTags, int numOfTopScoreEvents, int numOfSameTagEvents, int numOfRandomEvents)
    {
        List<EventItem> allSortedEvents = FilterMoodEventsByScoreSorted(in sourceEvents, selectedTags, 0);
        List<EventItem> returnEventsList = new List<EventItem>();

        List<EventItem> notUsedSortedEvents = allSortedEvents;
        if (notUsedSortedEvents.Count > numOfTopScoreEvents) 
            notUsedSortedEvents.RemoveRange(0, numOfTopScoreEvents - 1);

        List<EventItem> topScoreEventsCropped = CropEventsList(allSortedEvents, numOfTopScoreEvents);
        int topScoreEventsVar = numOfTopScoreEvents;
        topScoreEventsVar -= topScoreEventsCropped.Count;
        returnEventsList.AddRange(topScoreEventsCropped);

        int sameTagEventsVar = numOfSameTagEvents;
        if(notUsedSortedEvents.Count != 0)
        {
            List<EventItem> sameTagEvents = RequestRandomRecommendations(notUsedSortedEvents, sameTagEventsVar);
            sameTagEventsVar -= sameTagEvents.Count;
            returnEventsList.AddRange(sameTagEvents);
        }

        int randomEventsVar = numOfRandomEvents + sameTagEventsVar + topScoreEventsVar;
        List<EventItem> randomEvents = RequestRandomRecommendations(sourceEvents, randomEventsVar, returnEventsList);
        returnEventsList.AddRange(randomEvents);

        return returnEventsList;
    }

    /// <summary>
    /// This method searches the database for events which have the highest compatibility score with the requested tags
    /// </summary>
    /// <param name="sourceEvents">List of events that need to be filtered</param>
    /// <param name="selectedTags">An array of mood tags with weight</param>
    /// <param name="numOfRecommendations">Number of events that you need. Set to '0' if you want to get all events</param>
    /// <returns>List of events which have at least one tag from the given array of tags and sorted so that the events with highest compatibility score are in the beginning of the list</returns>
    public List<EventItem> FilterMoodEventsByScoreSorted(in List<EventItem> sourceEvents, MoodTagItem[] selectedTags, int numOfRecommendations)
    {
        List<EventItem> recommendedEvents = FilterMoodEventsContainingTagsUnsorted(in sourceEvents, selectedTags, 0);
        recommendedEvents = SortItemsByScore(recommendedEvents, selectedTags);
        recommendedEvents = CropEventsList(recommendedEvents, numOfRecommendations);
        return recommendedEvents;

    }
    private List<EventItem> SortItemsByScore(List<EventItem> selectedEvents, MoodTagItem[] selectedTags)
    {
        Dictionary<EventItem, float> eventsScored = new Dictionary<EventItem, float>();

        for (int i = 0; i < selectedEvents.Count; i++)
        {
            float eventScore = 0f;
            for (int k = 0; k < selectedEvents[i].moodTags.Count; k++)
            {
                for (int j = 0; j < selectedTags.Length; j++)
                {
                    if (selectedEvents[i].moodTags[k].tagName == selectedTags[j].tagName)
                    {
                        float tagScore = selectedEvents[i].moodTags[k].weight * selectedTags[j].weight;
                        eventScore += tagScore;
                    }
                }
            }
            eventsScored.Add(selectedEvents[i], eventScore);

        }
        var mySortedList = eventsScored.OrderByDescending(d => d.Value).ToList();
        List<EventItem> returnEvents = (from kvp in mySortedList select kvp.Key).Distinct().ToList();
        return returnEvents;
    }

    /// <summary>
    /// This method searches the database for events which contain requested mood tags
    /// </summary>
    /// <param name="sourceEvents">List of events that needs to be sorted</param>
    /// <param name="selectedTags">An array of mood tags</param>
    /// <param name="numOfRecommendations">Number of events that you need. Set to '0' if you want to get all events</param>
    /// <returns>List of events which have at least one tag from the given array of tags</returns>
    public List<EventItem> FilterMoodEventsContainingTagsUnsorted(in List<EventItem> sourceEvents, MoodTagItem[] selectedTags, int numOfRecommendations)
    {
        List<EventItem> foundEvents = sourceEvents.Where(p => p.moodTags.Any(value => selectedTags.Any(f => f.tagName == value.tagName))).ToList();

        foundEvents = CropEventsList(foundEvents, numOfRecommendations);

        return foundEvents;
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

    /// <summary>
    /// This method generated a required number of events from the given list
    /// </summary>
    /// <param name="source">Range of events to generate from</param>
    /// <param name="numOfRecommendations">Number of events that have to be generated</param>
    /// <param name="except">Generated items should not be the same as the items in this list</param>
    /// <returns>A list of events in random order</returns>
    public List<EventItem> RequestRandomRecommendations(List<EventItem> source, int numOfRecommendations, List<EventItem> except = null)
    {
        // -----!!! Later change to Hash Table for less sorting costs !!!----
        List<EventItem> randomEvents = new List<EventItem>();

        int amountOfRandGenerations = numOfRecommendations;
        if(source.Count < numOfRecommendations)
        {
            amountOfRandGenerations = source.Count;
        }
        for (int i = 0; i < amountOfRandGenerations; i++)
        {
            EventItem eventItem = FindRandomEvent(source);

            if (except != null)
            {
                if(except.Contains(eventItem))
                {
                    amountOfRandGenerations++;
                    continue;
                }
            }

            if (randomEvents.Contains(eventItem))
            {
                amountOfRandGenerations++;
                continue;
            }
            randomEvents.Add(eventItem);
        }

        return randomEvents;

    }
    private EventItem FindRandomEvent(List<EventItem> source)
    {
        EventItem returnEventItem = source[Random.Range(0, source.Count)];
        return returnEventItem;
    }

    #endregion
}
