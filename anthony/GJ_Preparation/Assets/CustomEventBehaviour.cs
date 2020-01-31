using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class CustomEventBehaviour<T> : MonoBehaviour where T : struct, IConvertible, IComparable, IFormattable
{

    private static Dictionary<T, UnityAction> eventActionsDict = new Dictionary<T, UnityAction>();

    public static void OnAction(T playAction)
    {
        if (eventActionsDict.ContainsKey(playAction))
        {
            foreach (var i in eventActionsDict)
            {
                i.Value.Invoke();
            }
        }
        else
        {
            Debug.Log("No actions correspond to this Event");
        }
    }

    //Set any kind of event.
    public void SetAction(T eventsEnum, UnityAction action)
    {
        if (eventActionsDict.ContainsKey(eventsEnum))
        {
            //Adding Action To dict
            eventActionsDict[eventsEnum] += action;
        }
        else
        {
            //Creating a new Key for Dict & add new action. 
            eventActionsDict.Add(eventsEnum, action);
        }
    }

    public void RemoveEvent(T eventsEnum, UnityAction action)
    {
        if (eventActionsDict.ContainsKey(eventsEnum))
        {
            eventActionsDict[eventsEnum] -= action;
        }
    }
}