using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventLearningManager
{
    private List<RecordOfRecommendations> outputedSetsOfEvents = new List<RecordOfRecommendations>();
    private int _idVar = 0;

    //Settings
    //-----!!! Later Change To scriptable Object !!!------------
    private float _basicEventIgnorePenalty;
    private float _basicEventPickReward;
    public EventLearningManager(float basicEventPickReward, float basicEventIgnorePenalty)
    {
        _basicEventPickReward = basicEventPickReward;
        _basicEventIgnorePenalty = basicEventIgnorePenalty;
    }

    public SetOfEvents RecordSetOfEvents(RecordOfRecommendations record)
    {
        _idVar++;
        record.setOfEvents._id = _idVar;
        outputedSetsOfEvents.Add(record);
        return record.setOfEvents;
    }

    public void AdjustScoresClientPick(int _setID, EventItem pickedItem)
    {
        RecordOfRecommendations setForModification = outputedSetsOfEvents.First(p => p.setOfEvents._id == _setID);
        //Debugging
        if(setForModification == null)
        {
            Debug.LogWarning($"No events sets with such ID:{_setID} have been recorded!");
            return;
        }
        if(setForModification.clientMood == null)
        {
            Debug.LogWarning($"There is not moods data for set with ID: {_setID}");
            return;
        }
        foreach (EventItem eventItem in setForModification.setOfEvents.recommendedEvents)
        {
            Debug.Log($"eventItem to be modifyed :{eventItem.eventName}");
            if(eventItem.eventName == pickedItem.eventName)
            {
                ModifyMoodScores(eventItem, setForModification.clientMood, _basicEventPickReward);
                CleanUpEvent(eventItem);
            }
            else
            {
                ModifyMoodScores(eventItem, setForModification.clientMood, _basicEventIgnorePenalty);
                CleanUpEvent(eventItem);
            }
        }
    }

    private EventItem ModifyMoodScores(EventItem oldEvent, List<MoodTagItem> userMoods, float rewardAmount)
    {
        Debug.LogWarning("Event Scores Modified");
        
        if (userMoods == null) return oldEvent;
        foreach (MoodTagItem userMood in userMoods)
        {
            Debug.Log($"Usermood:{userMood.tagName}, score:{userMood.weight}");
            MoodTagItem moodTagVar = new MoodTagItem();
            if(oldEvent != null)
                moodTagVar = oldEvent.moodTags.FirstOrDefault(p => p.tagName == userMood.tagName);
            MoodTagItem newMoodTag = new MoodTagItem();
            if (moodTagVar != null)
            {
                newMoodTag = new MoodTagItem(moodTagVar.tagName, moodTagVar.weight);
                float newScore = userMood.weight * rewardAmount;
                newMoodTag.weight += newScore;
                
                var index = oldEvent.moodTags.IndexOf(moodTagVar);
                oldEvent.moodTags[index] = newMoodTag;
                
            }
            else
            {
                newMoodTag = new MoodTagItem(userMood.tagName, userMood.weight);
                float newScore = userMood.weight * rewardAmount;
                newMoodTag.weight = newScore;

                oldEvent.moodTags.Add(newMoodTag);
            }
        }

        return oldEvent;
    }

    private void CleanUpEvent(EventItem eventItem)
    {
        foreach (MoodTagItem moodTag in eventItem.moodTags.ToArray())
        {
            if(moodTag.weight<= 0.001f)
            {
                eventItem.moodTags.Remove(moodTag);
            }
        }
    }

}
