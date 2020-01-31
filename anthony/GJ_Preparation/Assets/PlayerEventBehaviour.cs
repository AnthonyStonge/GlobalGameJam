using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventBehaviour : CustomEventBehaviour<PlayerEventBehaviour.Event>
{
    public enum Event
    {
        SPAWN,
        WALK,
        DIE,
        RUN,
        HIT,
        JUMP
    }

    [SerializeField] private CustomEvent onSpawnEvent;
    [SerializeField] private CustomEvent onWalkEvent;
    [SerializeField] private CustomEvent onDieEvent;
    [SerializeField] private CustomEvent onRunEvent;
    [SerializeField] private CustomEvent onHitEvent;
    [SerializeField] private CustomEvent onJumpEvent;

    private void PreInitialize()
    {
        onSpawnEvent = new CustomEvent();
        onWalkEvent = new CustomEvent();
        onDieEvent = new CustomEvent();
        onRunEvent = new CustomEvent();
        onHitEvent = new CustomEvent();
        onJumpEvent = new CustomEvent();

        SubscribeCustomEvent(Event.SPAWN, onSpawnEvent);
        SubscribeCustomEvent(Event.WALK, onWalkEvent);
        SubscribeCustomEvent(Event.DIE, onDieEvent);
        SubscribeCustomEvent(Event.RUN, onRunEvent);
        SubscribeCustomEvent(Event.HIT, onHitEvent);
        SubscribeCustomEvent(Event.JUMP, onJumpEvent);

        AddAction(Event.SPAWN, Spawn);
        AddAction(Event.WALK, Walk);
        AddAction(Event.DIE, Die);
        AddAction(Event.RUN, Run);
        AddAction(Event.HIT, Hit);
        AddAction(Event.JUMP, Jump);
    }

    private void OnDestroy()
    {
        onSpawnEvent.RemoveAllListeners();
        onWalkEvent.RemoveAllListeners();
        onDieEvent.RemoveAllListeners();
        onRunEvent.RemoveAllListeners();
        onHitEvent.RemoveAllListeners();
        onJumpEvent.RemoveAllListeners();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnAction(Event.HIT);
    }

    private void Jump()
    {
        Debug.Log("JUMP!");
    }

    private void Spawn()
    {
    }

    private void Hit()
    {
    }

    private void Run()
    {
    }

    private void Walk()
    {
    }

    private void Die()
    {
    }

    private void Awake()
    {
        PreInitialize();
    }
}