using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : IFlow
{
    
    #region Singleton
    private static Game instance = null;

    //Do not use the constructor.
    private Game() {}
    
    public static Game Instance => instance ?? (instance = new Game());
    #endregion
    
    public void PreInitialize()
    {
    }
    
    public void Initialize()
    {
    }
    
    public void Refresh()
    {
        if(Input.GetKeyDown(KeyCode.A))
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
    }
}
