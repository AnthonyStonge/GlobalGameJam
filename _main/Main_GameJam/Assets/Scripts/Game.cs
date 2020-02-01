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
    private UiManager uiManager;
    private enum GameState
    {
        Start,
        InGame,
        EndGame
    }
    private GameState gameState;
    public void PreInitialize()
    {
        gameState = GameState.Start;
        playerManager = PlayerManager.Instance;
        uiManager = UiManager.Instance;
        playerManager.PreInitialize();
    }

    public void Initialize()
    {
        playerManager.Initialize();
    }

    public void Refresh()
    {
        switch (gameState)
        {
            case GameState.Start:
                
                break;
            case GameState.InGame:
                break;
            case  GameState.EndGame:
                break;
        }
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