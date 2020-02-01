using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerEvents : CustomEventBehaviour<PlayerEvents.Event>, IFlow
{
    public enum Event
    {
        DASH,
        DIE,
        TRHOW,
        START_MOVING,
        STOP_MOVING,
        FOOT_STEP_LEFT,
        FOOT_STEP_RIGHT
    }

    [Header("Settings")] public float speed = 100;

    [Header("Internal")] public Rigidbody rb;
    public Transform shotSpawn;
    private GameObject bulletPrefab;

    [SerializeField] private CustomEvent onDash;
    [SerializeField] private CustomEvent onDie;
    [SerializeField] private CustomEvent onThrow;
    [SerializeField] private CustomEvent onStartMoving;
    [SerializeField] private CustomEvent onStopMoving;
    [SerializeField] private VisualEffect footStepLeft;
    [SerializeField] private VisualEffect footStepRight;

    private Animator animator;
    private Vector2 currentInput;
    private bool isMoving = false;
    private int AssID;
    public bool eggCompleted;
    public int numberEgg;

    public void PreInitialize()
    {
        bulletPrefab = Resources.Load<GameObject>("Prefabs/Bullet/Egg");
        eggCompleted = true;
        this.numberEgg = 2;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        onDash = new CustomEvent();
        onDie = new CustomEvent();
        onThrow = new CustomEvent();
        onStartMoving = new CustomEvent();
        onStopMoving = new CustomEvent();

        currentInput = new Vector2();

        SubscribeCustomEvent(Event.DIE, onDie);
        SubscribeCustomEvent(Event.DASH, onDash);
        SubscribeCustomEvent(Event.TRHOW, onThrow);
        SubscribeCustomEvent(Event.START_MOVING, onStartMoving);
        SubscribeCustomEvent(Event.STOP_MOVING, onStopMoving);

        AddAction(Event.DIE, Die);
        AddAction(Event.DASH, Dash);
        AddAction(Event.TRHOW, Throw);
        AddAction(Event.START_MOVING, StartMoving);
        AddAction(Event.STOP_MOVING, StopMoving);
        AddAction(Event.FOOT_STEP_LEFT, Foot_Step_Left);
        AddAction(Event.FOOT_STEP_RIGHT, Foot_Step_Right);
    }

    public void Initialize()
    {
    }

    public void Refresh()
    {
    }

    public void PhysicsRefresh()
    {
    }

    public void LateRefresh()
    {
    }

    public void EndFlow()
    {
    }

    private void OnDestroy()
    {
        onDash.RemoveAllListeners();
        onDie.RemoveAllListeners();
        onThrow.RemoveAllListeners();
    }

    public void Move(float horizontal, float vertical)
    {
        //TODO minimum speed 
        //TODO maximum speed

        //Block movement if player not really pushing the joystick.
        if ((horizontal < 0.01f && vertical < 0.01f) && (horizontal > -0.01f && vertical > -0.01f))
        {
            if (isMoving)
            {
                OnAction(Event.STOP_MOVING);
                isMoving = false;
            }
        }
        else
        {
            if (!isMoving)
            {
                OnAction(Event.START_MOVING);
                isMoving = true;
            }

            var newDirection = Quaternion.LookRotation(new Vector3(horizontal, 0, vertical)).eulerAngles;

            newDirection.x = 0;
            newDirection.z = 0;
            transform.rotation = Quaternion.Euler(newDirection);

            rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }
    }

    public void StartMoving()
    {
        animator.SetBool("Run", true);
    }

    public void StopMoving()
    {
        animator.SetBool("Run", false);
    }

    public void Foot_Step_Left()
    {
        this.footStepLeft.Play();
    }
    public void Foot_Step_Right()
    {
        this.footStepRight.Play();
    }

    public void PlayHitSound()
    {
        SoundManager.Instance.PlayOnce(gameObject, 0);
    }
    public void Die()
    {
        Debug.Log("In Die");
        animator.SetTrigger("Die");
        Game.Instance.gameState = Game.GameState.EndGame;
    }

    public void Dash()
    {
        Debug.Log("In Dash");
    }

    public void Throw()
    {
        if (eggCompleted)
        {
            GameObject shot = GameObject.Instantiate(bulletPrefab, shotSpawn.position, shotSpawn.rotation);
            Bullet bullet = shot.GetComponent<Bullet>();
            bullet.Initialize(AssID);
            bullet.Launch(transform);
            eggCompleted = false;
            this.numberEgg = 0;
        }
    }


    void IFlow.PreInitialize()
    {
        PreInitialize();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 direction = transform.forward * 5;
        Gizmos.DrawRay(transform.position, direction);
    }

    public void SetAssID(int id)
    {
        AssID = id;
        transform.tag = id.ToString();
    }
}