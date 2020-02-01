using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet_Half : MonoBehaviour, IPickable
{
    private Rigidbody rb;

    public float timeBeforeFrozen = 5f;

    private void Awake()
    {
        TimeManager.Instance.PreInitialize();
        //TODO remove line under
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        TimeManager.Instance.Refresh();
    }

    public void Initialize()
    {
        this.rb = GetComponent<Rigidbody>();
        TimeManager.Instance.AddTimedAction(new TimedAction(() => { this.rb.isKinematic = true; }, this.timeBeforeFrozen));
    }


    public void PickedUp()
    {
        
    }
}