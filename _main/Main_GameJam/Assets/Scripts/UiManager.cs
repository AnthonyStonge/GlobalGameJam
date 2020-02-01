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

    public bool beginGame = false;
    public void PreInitialize()
    {
        
    }

    public void Initialize()
    {
        
    }

    public void Refresh()
    {
        if (beginGame)
        {
            
        }
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
    
}
