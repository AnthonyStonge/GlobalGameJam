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
    private TimeManager timeManager;
    private CamController camController;
    public void PreInitialize()
    {
        camController = GameObject.FindWithTag("CamController").GetComponent<CamController>();
        uiPrincipal = GameObject.FindWithTag("Ui").GetComponent<UiPrincipal>();
        timeManager = TimeManager.Instance;
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
        timeManager.AddTimedAction(new TimedAction(uiPrincipal.StartGame, 4f));
        uiPrincipal.gameUiActivator();
        camController.camState = CamController.CamState.Transition;
        //uiPrincipal.StartGame();
    }
    public void SetCamFov()
    {
        camController.camState = CamController.CamState.InGame;
    }
    public void SetEndGame(bool playerOneWin)
    {
        uiPrincipal.EndGame(playerOneWin);
        camController.camState = CamController.CamState.EndGame;
    }

    public void SetMenu()
    {
        uiPrincipal.StartMenu();
    }
}
