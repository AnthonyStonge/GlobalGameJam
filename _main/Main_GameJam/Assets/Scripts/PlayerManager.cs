using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;


public class PlayerManager : IFlow
{
    #region Singleton

    private static PlayerManager instance = null;

    //Do not use the constructor.
    private PlayerManager()
    {
    }

    public static PlayerManager Instance => instance ?? (instance = new PlayerManager());

    #endregion

    public GameObject playerGameObject;
    public PlayerInputs player;
    
    public void PreInitialize()
    {
        playerGameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player = playerGameObject.GetComponent<PlayerInputs>();
        player.PreInitialize();
    }

    public void Initialize()
    {
        player.Initialize();
    }

    public void Refresh()
    {
        player.Refresh();
    }

    public void PhysicsRefresh()
    {
        player.PhysicsRefresh();
    }

    public void LateRefresh()
    {
        player.LateRefresh();
    }

    public void EndFlow()
    {
        player.EndFlow();
    }
}