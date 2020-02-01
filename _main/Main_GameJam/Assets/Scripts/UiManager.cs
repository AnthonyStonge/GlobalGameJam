using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : IFlow
{
    // Start is called before the first frame update
    #region Singleton

    private static UiManager instance = null;

    //Do not use the constructor.
    private UiManager()
    {
    }

    public static UiManager Instance => instance ?? (instance = new UiManager());

    #endregion
    
    private UiPrincipal uiPrincipal;
    public void PreInitialize()
    {
        uiPrincipal = GameObject.FindWithTag("Ui").GetComponent<UiPrincipal>();
    }

    public void Initialize()
    {
        
    }

    public void Refresh()
    {
        if(Game.Instance.gameState == Game.GameState.Start)
            uiPrincipal.CountDown();
        
    }

    public void PhysicsRefresh()
    {
        
    }

    public void LateRefresh()
    {
        
    }

    public void EndFlow()
    {
        throw new System.NotImplementedException();
    }

    public void SetBeginGame()
    {
        uiPrincipal.StartGame();
    }
    public void SetEndGame(bool playerOneWin)
    {
        uiPrincipal.EndGame(playerOneWin);
    }

    public void SetMenu()
    {
        uiPrincipal.StartMenu();
    }
}
