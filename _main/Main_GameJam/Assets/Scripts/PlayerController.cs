using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerController : MonoBehaviour, IFlow
{
    [Header("Settings")] 
    public float speed; 
    
    [Header("Internal")]
    public Rigidbody rb;
    
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
    }

    public void PhysicsRefresh()
    {
        float moveHorizontal = player.GetAxis("Horizontal");
        float moveVertical = player.GetAxis("");

        Vector3 movement = new Vector3(-moveHorizontal, 0, -moveVertical);
        rb.AddForce(movement * speed);
        
        //Jump
        if (player.GetButtonDown(""))
        {
            //TODO
        }
        
    }

    public void LateRefresh()
    {
    }

    public void EndFlow()
    {
    }
}
