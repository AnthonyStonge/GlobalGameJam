using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerInputs : MonoBehaviour, IFlow
{
    [Header("Settings")] public float speed;

    [Header("Internal")] public Rigidbody rb;

    [SerializeField] private int playerID = 0;
    [SerializeField] private Player player;


    public void PreInitialize()
    {
        player = ReInput.players.GetPlayer(playerID);

        if (!rb)
        {
            Debug.LogWarning("Player does not have a rigidbody.");
        }
    }

    public void Initialize()
    {
    }

    public void Refresh()
    {
        float moveHorizontal = player.GetAxis(0);
        float moveVertical = player.GetAxis(1);


        if (player.GetButtonDown("Shoot"))
        {
            Debug.Log("Shooting");
        }

        Debug.Log("Move Vertical : " + moveVertical);
        Debug.Log("Move Horizontal : " + moveHorizontal);

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(movement * speed);

        //Jump
        if (player.GetButtonDown(""))
        {
            //TODO
        }
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
}
