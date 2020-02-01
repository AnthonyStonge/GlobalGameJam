


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class CustomEvent : UnityEvent
{
}

public abstract class CustomEventBehaviour<T> : MonoBehaviour where T : struct, IConvertible, IComparable, IFormattable
{
    private Dictionary<T, UnityEvent> eventActionsDict = new Dictionary<T, UnityEvent>();

    //Can only subscribe one event per T : Enum
    protected void SubscribeCustomEvent(T eEvent, UnityEvent customEvent)
    {
        if (!eventActionsDict.ContainsKey(eEvent))
        {
            eventActionsDict.Add(eEvent, customEvent);
        }
        else
        {
            Debug.LogError(this + "Already contains this Key!");
        }
    }

    public void OnAction(T playAction)
    {
        if (eventActionsDict.ContainsKey(playAction))
        {
            eventActionsDict[playAction].Invoke();
        }
        else
        {
            Debug.LogWarning(this + "No actions correspond to this Event");
        }
    }

    //Set any kind of event.
    public void AddAction(T eventsEnum, UnityAction action)
    {
        if (eventActionsDict.ContainsKey(eventsEnum))
        {
            //Adding Action To dict
            eventActionsDict[eventsEnum].AddListener(action);
        }
        else
        {
            Debug.LogWarning(this + " : Event does not exist.");
        }
    }

    public void RemoveAction(T eventsEnum, UnityAction action)
    {
        if (eventActionsDict.ContainsKey(eventsEnum))
        {
            eventActionsDict[eventsEnum].RemoveListener(action);
        }
        else
        {
            Debug.LogWarning(this + " : Event does not exist.");
        }
    }
}