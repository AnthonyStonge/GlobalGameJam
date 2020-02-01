using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Menu : IFlow
{
    #region Singleton

    private static Menu instance = null;
    
    private Menu()
    {
    }

    public static Menu Instance => instance ?? (instance = new Menu());

    #endregion
    CinemachineVirtualCamera vcCam;
    private UiManager uiManager;
    public void PreInitialize()
    {
        uiManager = UiManager.Instance;
        vcCam = GameObject.FindWithTag("CamBrain").GetComponent<CinemachineVirtualCamera>();
        uiManager.PreInitialize();
    }

    public void Initialize()
    {
        uiManager.SetMenu();
    }

    public void Refresh()
    {
        
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
