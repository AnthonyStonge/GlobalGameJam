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
    private TimeManager timeManager;
    public bool playerOneWin = false;
    public enum GameState
    {
        Start,
        InGame,
        EndGame
    }
    public GameState gameState;
    public void PreInitialize()
    {
        gameState = GameState.Start;
        playerManager = PlayerManager.Instance;
        uiManager = UiManager.Instance;
        timeManager = TimeManager.Instance;
        
        playerManager.PreInitialize();
        uiManager.PreInitialize();
        timeManager.PreInitialize();
        
    }

    public void Initialize()
    {
        gameState = GameState.Start;
        playerManager.Initialize();
        uiManager.SetBeginGame();
    }

    public void Refresh()
    {
        uiManager.Refresh();
        switch (gameState)
        {
            case GameState.Start:
                playerManager.canShoot1 = false;
                playerManager.canShoot2 = false;
                break;
            case GameState.InGame:
                playerManager.player1Events.gameOver = false;
                playerManager.player2Events.gameOver = false;
                
                playerManager.canShoot1 = true;
                playerManager.canShoot2 = true;
                break;
            case  GameState.EndGame:
                playerManager.canShoot1 = false;
                playerManager.canShoot2 = false;
                
                uiManager.SetEndGame(playerOneWin);
                playerManager.player1Events.ResetValues();
                playerManager.player2Events.ResetValues();
                break;
        }
        playerManager.Refresh();

        timeManager.Refresh();
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