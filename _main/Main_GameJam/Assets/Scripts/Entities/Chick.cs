﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : MonoBehaviour
{
    public float speed = 1f;
    public Transform lookAt1;
    public Transform lookAt2;
    private Animator animator;
    private Vector3 targetPosition;
    public bool firstPlayer;
    private bool isMovingToward;
    private bool isReparing;


    public void Initialize()
    {
        lookAt1 = GameObject.FindGameObjectWithTag("LookAt1").transform;
        lookAt2 = GameObject.FindGameObjectWithTag("LookAt2").transform;
        isMovingToward = true;
        isReparing = false;
        this.animator = GetComponent<Animator>();
    }

    public void Refresh()
    {
        if (isReparing)
        {
            isReparing = false;
            animator.SetTrigger("Attack");

            TimeManager.Instance.AddTimedAction(new TimedAction(
                () => { this.isReparing = true; }, 1f
            ));
        }
    }

    public void PhysicRefresh()
    {
        if (isMovingToward)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, this.targetPosition, this.speed * Time.fixedDeltaTime);
            if(firstPlayer)
                transform.LookAt(lookAt1);
            else
                transform.LookAt(lookAt2);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            isReparing = true;
        }

        //Check if at position
        if (isMovingToward && transform.position == this.targetPosition)
        {
            isMovingToward = false;
            if(firstPlayer)
                transform.LookAt(lookAt1);
            else
                transform.LookAt(lookAt2);
        }
    }

    public void Win()
    {
        //this.animator.SetTrigger("");
    }

    public void Loose()
    {
        this.animator.SetTrigger("Die");
    }

    public void SetCannonPosition(Vector3 position)
    {
        transform.forward = position;
        this.targetPosition = position;
    }
}