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
    [Header("Settings")]
    public float speed = 100;
    
    [Header("Internal")]
    public Rigidbody rb;
    public GameObject bulletPrefab;
    public Transform shotSpawn;

    [SerializeField] private CustomEvent onDash;
    [SerializeField] private CustomEvent onDie;
    [SerializeField] private CustomEvent onThrow;
    
   
    private void PreInitialize()
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/NotBullet");
        
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

    public void Move(float horizontal,float vertical)
    {
        //TODO When not moving but has momentum
        //TODO minimum speed 
        //TODO maximum speed
        Vector3 movement = new Vector3(horizontal, 0, vertical);
        rb.AddForce(movement * speed);
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

        GameObject shot = Instantiate(bulletPrefab, shotSpawn);
        shot.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(100,0,0),ForceMode.Force);
        Debug.Log("In Throw");

    }
    
    
}
