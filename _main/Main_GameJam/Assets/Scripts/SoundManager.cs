using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private PlayerEvents player;

    public string Sound1 = "SON"; 
    
    // Start is called before the first frame update
    void Start()
    {
        player.AddAction(PlayerEvents.Event.DIE, () => { Sound("D"); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Test1()
    {
        
    }

    private void Sound(string nomDuSon)
    {
        //Jouer son
    }
}
