using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyEventBehaviour : CustomEventBehaviour<EnemyEventBehaviour.Event>
{
   
    public enum Event
    {
       ATTACK,
       DEFEND
    }

    public UnityEvent onAttackEvent;
    public UnityEvent onDefendEvent;

    private void PreInitialize()
    {
        SubscribeCustomEvent(Event.ATTACK, onAttackEvent);
        SubscribeCustomEvent(Event.DEFEND, onDefendEvent);
    }

    private void OnDestroy()
    {
        onAttackEvent.RemoveAllListeners();
        onDefendEvent.RemoveAllListeners();
    }

    private void Awake()
    {
        PreInitialize();    
    }
}
