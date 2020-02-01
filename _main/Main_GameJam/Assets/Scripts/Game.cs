using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class Game : IFlow
{
    #region Singleton

    private static Game instance = null;
    
    private Game()
    {
    }

    public static Game Instance => instance ?? (instance = new Game());

    #endregion

    private PlayerManager playerManager;

    public void PreInitialize()
    {
        playerManager = PlayerManager.Instance;

        playerManager.PreInitialize();
    }

    public void Initialize()
    {
        playerManager.Initialize();
    }

    public void Refresh()
    {
        playerManager.Refresh();
    }

    public void PhysicsRefresh()
    {
        playerManager.PhysicsRefresh();
    }

    public void LateRefresh()
    {
        playerManager.LateRefresh();
    }

    public void EndFlow()
    {
        playerManager.EndFlow();
    }
}