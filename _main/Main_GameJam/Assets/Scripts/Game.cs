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
    public enum GameState
    {
        Start,
        InGame,
        EndGame
    }
    public GameState gameState;
    public void PreInitialize()
    {
        gameState = GameState.EndGame;
        playerManager = PlayerManager.Instance;
        uiManager = UiManager.Instance;
        playerManager.PreInitialize();
        uiManager.PreInitialize();
    }

    public void Initialize()
    {
        playerManager.Initialize();
    }

    public void Refresh()
    {
        uiManager.Refresh();
        switch (gameState)
        {
            case GameState.Start:
               
                break;
            case GameState.InGame:
                break;
            case  GameState.EndGame:
                uiManager.SetEndGame(true);
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