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
            HIT
        }

        public CustomEvent onSpawnEvent;
        public CustomEvent onWalkEvent;
        public CustomEvent onDieEvent;
        public CustomEvent onRunEvent;
        public CustomEvent onHitEvent;

        private void PreInitialize()
        {
            SubscribeCustomEvent(Event.SPAWN, onSpawnEvent);
            SubscribeCustomEvent(Event.WALK, onWalkEvent);
            SubscribeCustomEvent(Event.DIE, onDieEvent);
            SubscribeCustomEvent(Event.RUN, onRunEvent);
            SubscribeCustomEvent(Event.HIT, onHitEvent);
            
            AddAction(Event.SPAWN, Spawn);
            AddAction(Event.WALK, Walk);
            AddAction(Event.DIE, Die);
            AddAction(Event.RUN, Run);
            AddAction(Event.HIT, Hit);
            
            
            
        }

        private void OnDestroy()
        {
            onSpawnEvent.RemoveAllListeners();
            onWalkEvent.RemoveAllListeners();
            onDieEvent.RemoveAllListeners();
            onRunEvent.RemoveAllListeners();
            onHitEvent.RemoveAllListeners();
        }

        private void OnTriggerEnter(Collider other)
        {
            OnAction(Event.HIT);
            
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


