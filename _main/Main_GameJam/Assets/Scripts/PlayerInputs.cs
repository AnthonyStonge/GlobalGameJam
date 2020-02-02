using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.Serialization;

public class PlayerInputs : MonoBehaviour, IFlow
{
    [Header("Internal")]
    [SerializeField] private int playerID;
    [SerializeField] private Player player;
    [SerializeField] public PlayerEvents playerEvents;
    public bool canShoot = false;

    public void PreInitialize()
    {
        player = ReInput.players.GetPlayer(playerID);
    }

    public void Initialize()
    {
    }

    public void Refresh()
    {
        if (player.GetButtonDown("Shoot") && canShoot)
        {
            playerEvents.Throw();
        }
    }

    public void PhysicsRefresh()
    {
        float moveHorizontal = player.GetAxis(0);
        float moveVertical = player.GetAxis(1);
        playerEvents.Move(moveHorizontal,moveVertical);
        
        
      
    }

    public void LateRefresh()
    {
    }

    public void EndFlow()
    {
    }

    public void SetPlayerID(int id)
    {
        playerID = id;
    }
}
