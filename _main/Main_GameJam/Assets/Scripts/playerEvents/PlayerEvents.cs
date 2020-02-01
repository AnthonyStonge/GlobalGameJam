using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents : CustomEventBehaviour<PlayerEvents.Event>
{
    
    public enum Event
    {

        DASH,
        DIE,
        TRHOW
    }

    [SerializeField] private CustomEvent onDash;
    [SerializeField] private CustomEvent onDie;
    [SerializeField] private CustomEvent onThrow;
    public float speed = 100;
    public Rigidbody rb;
    private void PreInitialize()
    {
        rb = GetComponent<Rigidbody>();
        onDash = new CustomEvent();
        onDie = new CustomEvent();
        onThrow = new CustomEvent();
        
        SubscribeCustomEvent(Event.DIE,onDie);
        SubscribeCustomEvent(Event.DASH, onDash);
        SubscribeCustomEvent(Event.TRHOW,onThrow);
        
        AddAction(Event.DIE,Die);
        AddAction(Event.DASH,Dash);
        AddAction(Event.TRHOW, Throw);
    }

    private void OnDestroy()
    {
        onDash.RemoveAllListeners();
        onDie.RemoveAllListeners();
        onThrow.RemoveAllListeners();
    }

    private void Awake()
    {
        PreInitialize();
    }

    public void Move(float input,float secondInput)
    {
        Vector3 movement = new Vector3(input, 0, secondInput);
        rb.AddForce(movement * speed);
        Debug.Log("In Move");
    }



    public void Die()
    {
        Debug.Log("In Die");

    }

    public void Dash()
    {
        Debug.Log("In Dash");

    }

    public void Throw()
    {
        Debug.Log("In Throw");

    }
    
    
}
