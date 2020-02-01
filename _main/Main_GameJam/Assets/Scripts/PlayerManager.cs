using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
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

    public GameObject player1;
    public PlayerInputs player1Inputs;
    public Vector3 player1Position = new Vector3(-5,2f,0);

    public GameObject player2;
    public PlayerInputs player2Inputs;
    public Vector3 player2Position = new Vector3(5,2f,0);
    
    public void PreInitialize()
    {
        player1 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), player1Position, Quaternion.identity);
        player2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), player2Position, Quaternion.identity);
        
        player1Inputs = player1.GetComponent<PlayerInputs>();
        player2Inputs = player2.GetComponent<PlayerInputs>();

        player1Inputs.SetPlayerID(0);
        player2Inputs.SetPlayerID(1);
        
        player1Inputs.PreInitialize();
        player2Inputs.PreInitialize();
    }

    public void Initialize()
    {
        player1Inputs.Initialize();
        player2Inputs.Initialize();
    }

    public void Refresh()
    {
        player1Inputs.Refresh();
        player2Inputs.Refresh();
    }

    public void PhysicsRefresh()
    {
        player1Inputs.PhysicsRefresh();
        player2Inputs.PhysicsRefresh();
    }

    public void LateRefresh()
    {
        player1Inputs.LateRefresh();
        player2Inputs.LateRefresh();
    }

    public void EndFlow()
    {
        player1Inputs.EndFlow();
        player2Inputs.EndFlow();
    }
}