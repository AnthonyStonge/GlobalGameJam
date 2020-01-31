using System.Collections;
using System.Collections.Generic;
using UnityEngine;

   

    public class PlayerEventBehaviour : CustomEventBehaviour<PlayerEventBehaviour.Event>
    {
        public enum Event
        {
            SPAWN,
            WALK,
            DIE,
            RUN,
            HIT
        }
        
        
        
    }


