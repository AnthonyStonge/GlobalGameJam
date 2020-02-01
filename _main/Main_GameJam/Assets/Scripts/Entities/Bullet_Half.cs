using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Bullet_Half : MonoBehaviour, IPickable
{
    public int ID { get; private set; }
    private Rigidbody rb;

    public float timeBeforeFrozen = 5f;

    private void Update()
    {
        TimeManager.Instance.Refresh();
    }

    public void Initialize(int id)
    {
        this.ID = id;
        transform.tag = id.ToString();

        this.rb = GetComponent<Rigidbody>();
        //TimeManager.Instance.AddTimedAction(new TimedAction(() => { this.rb.isKinematic = true; }, this.timeBeforeFrozen));
    }


    public void PickedUp()
    {
        
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag(this.ID.ToString()))
        {
            PlayerEvents events = other.transform.GetComponent<PlayerEvents>();
            events.numberEgg++;
            if (events.numberEgg >= 2)
                events.eggCompleted = true;
            GameObject.Destroy(gameObject);

        }
    }
}