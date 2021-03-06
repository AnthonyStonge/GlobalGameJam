﻿using System.Collections;
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

    public GameObject player1;
    public PlayerEvents player1Events;
    public PlayerInputs player1Inputs;
    public Vector3 player1Position = new Vector3(272, 134, 416);
    public bool canShoot1;


    public GameObject player2;
    public PlayerEvents player2Events;
    public PlayerInputs player2Inputs;
    public Vector3 player2Position = new Vector3(270, 134, 457);
    public bool canShoot2;

    public List<Chick> chicksssss;

    public void PreInitialize()
    {
        this.chicksssss = new List<Chick>();

        player1 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Chicken"), player1Position,
            Quaternion.Euler(0, 90, 0));
        player2 = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Chicken"), player2Position,
            Quaternion.Euler(0, 90, 0));
        

        player1Inputs = player1.GetComponent<PlayerInputs>();
        player2Inputs = player2.GetComponent<PlayerInputs>();

        player1Events = player1.GetComponent<PlayerEvents>();
        player2Events = player2.GetComponent<PlayerEvents>();

        player1Inputs.SetPlayerID(0);
        player2Inputs.SetPlayerID(1);
        player1Events.SetAssID(10);
        player2Events.SetAssID(20);

        player1Inputs.PreInitialize();
        player2Inputs.PreInitialize();
        player1Events.PreInitialize();
        player2Events.PreInitialize();
    }

    public void Initialize()
    {
        Bullet_Half[] ball = GameObject.FindObjectsOfType<Bullet_Half>();
        foreach (Bullet_Half halfEgg in ball)
        {
            GameObject.Destroy(halfEgg.gameObject);
        }

        player1.transform.rotation = Quaternion.Euler(0, 90, 0);
        player2.transform.rotation = Quaternion.Euler(0, 90, 0);

        player1Events.eggCompleted = true;
        player2Events.eggCompleted = true;
        player1Inputs.canShoot = false;
        player2Inputs.canShoot = false;

        player1.transform.position = player2Position;
        player2.transform.position = player1Position;

        player1Inputs.Initialize();
        player2Inputs.Initialize();
        player1Events.Initialize();
        player2Events.Initialize();
    }

    public void Refresh()
    {
        player1Inputs.canShoot = canShoot1;
        player2Inputs.canShoot = canShoot2;

        player1Inputs.Refresh();
        player2Inputs.Refresh();
        player1Events.Refresh();
        player2Events.Refresh();

        foreach (Chick chick in this.chicksssss)
        {
            chick.Refresh();
        }
    }

    public void PhysicsRefresh()
    {
        player1Inputs.PhysicsRefresh();
        player2Inputs.PhysicsRefresh();
        player1Events.PhysicsRefresh();
        player2Events.PhysicsRefresh();
        
        foreach (Chick chick in this.chicksssss)
        {
            chick.PhysicRefresh();
        }
    }

    public void LateRefresh()
    {
        player1Inputs.LateRefresh();
        player2Inputs.LateRefresh();
        player1Events.LateRefresh();
        player2Events.LateRefresh();
    }

    public void EndFlow()
    {
        player1Inputs.EndFlow();
        player2Inputs.EndFlow();
        player1Events.EndFlow();
        player2Events.EndFlow();
    }

    public void ReinitializePosition()
    {
        player1.transform.position = player1Position;
        player2.transform.position = player2Position;
    }
}