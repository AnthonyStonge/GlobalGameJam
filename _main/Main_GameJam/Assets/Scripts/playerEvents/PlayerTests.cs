using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTests : MonoBehaviour
{

    public PlayerEvents player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            player.OnAction(PlayerEvents.Event.DIE);

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            player.OnAction(PlayerEvents.Event.DASH);

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            player.OnAction(PlayerEvents.Event.TRHOW);

        }
    }
}
