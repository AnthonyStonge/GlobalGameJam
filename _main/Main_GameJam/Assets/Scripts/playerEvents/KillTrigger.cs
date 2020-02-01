using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    public PlayerEvents player;
    private void OnTriggerEnter(Collider other)
    {
        player.OnAction(PlayerEvents.Event.DIE);
    }
}
