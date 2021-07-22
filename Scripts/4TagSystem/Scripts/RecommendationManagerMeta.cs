using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordOfRecommendations
{
    public SetOfEvents setOfEvents = new SetOfEvents();
    public List<MoodTagItem> clientMood = default;
}

public class SetOfEvents
{
    public int _id;
    public List<EventItem> recommendedEvents = new List<EventItem>();
}

public class RecommendationManagerMeta : MonoBehaviour
{
    [Header("Recommendation Learnin Settings")]
    public float _basicEventIgnorePenalty;
    public float _basicEventRewardPick;
    [Header("Mood Recommendation Settings")]
    public int topScorePercentage = 50;
    public int secondaryScorePercentage = 30;

    public static List<EventItem> eventsDatabase = new List<EventItem>();
    private CategoryFilteringManager categoryRecommendationManager = default;
    private MoodFilteringManager moodRecommendationManager = default;
    private EventLearningManager eventLearningManager = default;

    private void Awake()
    {
        LoadDatabase();
        categoryRecommendationManager = new CategoryFilteringManager();
        moodRecommendationManager = new MoodFilteringManager(topScorePercentage, secondaryScorePercentage);
        eventLearningManager = new EventLearningManager(_basicEventRewardPick, _basicEventIgnorePenalty);
    }

    private void LoadDatabase()
    {
        eventsDatabase = new List<EventItem>(Resources.LoadAll<EventItem>("Events"));
        //Debug.Log($"All events : {eventsDatabase.Count}");
    }

    public SetOfEvents RequestEvents(int amountOfEvents, List<MoodTagItem> moodTags = null, List<CategoryTagItem> categoryTags = null, List<EventItem> eventsToSort = null)
    {
        //----!!! Neither moodTags and Category tags it throws an error
        RecordOfRecommendations returnSetOfEvents = new RecordOfRecommendations();
        returnSetOfEvents.clientMood = moodTags;
        if(eventsToSort != null)
        {
            returnSetOfEvents.setOfEvents.recommendedEvents = eventsToSort;
        }
        else
        {
            returnSetOfEvents.setOfEvents.recommendedEvents = eventsDatabase;
        }
        
        if(categoryTags == null && moodTags == null)
        {
            returnSetOfEvents.setOfEvents.recommendedEvents = moodRecommendationManager.RequestRandomRecommendations(returnSetOfEvents.setOfEvents.recommendedEvents, amountOfEvents);
        }
        if(categoryTags != null)
        {
            if(moodTags!= null)
            {
                returnSetOfEvents.setOfEvents.recommendedEvents = categoryRecommendationManager.FilterSetOfCategoryEvents(returnSetOfEvents.setOfEvents.recommendedEvents, categoryTags.ToArray(), 0);
            }
            else
            {
                returnSetOfEvents.setOfEvents.recommendedEvents = categoryRecommendationManager.FilterSetOfCategoryEvents(returnSetOfEvents.setOfEvents.recommendedEvents, categoryTags.ToArray(), amountOfEvents);
            }
        }
        if(moodTags != null)
        {
            returnSetOfEvents.setOfEvents.recommendedEvents = moodRecommendationManager.RequestMoodEvents(returnSetOfEvents.setOfEvents.recommendedEvents, moodTags, amountOfEvents);
            
        }

        SetOfEvents returnSet = eventLearningManager.RecordSetOfEvents(returnSetOfEvents);
        return returnSet;
    }

    public void OnEventPicked(EventItem eventItem, int setId)
    {
        Debug.Log("On Event Picked");
        eventLearningManager.AdjustScoresClientPick(setId, eventItem);
    }

    public List<EventItem> TestMethodCategory(List<CategoryTagItem> categoryTags)
    {
        List<EventItem> events = categoryRecommendationManager.FilterSetOfCategoryEvents(eventsDatabase, categoryTags.ToArray() ,0);
        return events;
    }

}
