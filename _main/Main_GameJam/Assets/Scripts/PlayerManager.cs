using System.Collections;
using System.Collections.Generic;
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

    public void PreInitialize()
    {
    }

    public void Initialize()
    {
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