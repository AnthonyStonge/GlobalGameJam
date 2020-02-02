using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet_Half : MonoBehaviour
{
    public int ID { get; private set; }
    private Rigidbody rb;

    public float timeBeforeFrozen = 5f;

    public void Initialize(int id)
    {
        this.ID = id;
        transform.tag = id.ToString();

        this.rb = GetComponent<Rigidbody>();
        //TimeManager.Instance.AddTimedAction(new TimedAction(() => { this.rb.isKinematic = true; }, this.timeBeforeFrozen));
        
        //Init color
        foreach (MeshRenderer mesh in transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (this.ID == 10)
            { 
                mesh.material = Resources.Load<Material>("Material/EggPlayer_1");
            }
               
            else if (this.ID == 20)
            {
                mesh.material = Resources.Load<Material>("Material/EggPlayer_2");
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(this.ID.ToString()))
        {
            PlayerEvents events = other.transform.GetComponent<PlayerEvents>();
            events.numberEgg++;
            if (events.numberEgg >= 2)
            {
                events.eggCompleted = true;
                events.canShoot = true;
                events.PlayerGlow();
            }
            
                
            GameObject.Destroy(gameObject);

        }
    }
}