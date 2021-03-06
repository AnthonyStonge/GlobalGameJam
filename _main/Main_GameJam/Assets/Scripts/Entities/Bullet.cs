﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public float throwForce = 300;
    public float explosionForce = 10;
    public float explosionRadius = 2;
    public Vector3 offsetExplosion;
    public float delayCollider = 0.3f;

    public GameObject bulletTopPart;
    public GameObject bulletBottomPart;

    private Transform spawnPoint1, spawnPoint2;
    private Rigidbody rb;
    private SphereCollider[] colliders;

    private int id;
    private bool hasBroken;


    public void Initialize(int id)
    {
        this.id = id;
        this.hasBroken = false;

        this.rb = transform.GetComponent<Rigidbody>();
        this.colliders = transform.GetComponentsInChildren<SphereCollider>();

        int bulletLayer = 1 << LayerMask.GetMask("Bullet");
        int defaultLayer = 1 << LayerMask.GetMask("Default");
        Physics.IgnoreLayerCollision(defaultLayer, bulletLayer, true);

        TimeManager.Instance.AddTimedAction(
            new TimedAction(() => { Physics.IgnoreLayerCollision(defaultLayer, bulletLayer, false); }, 0.1f)
        );


        //Get spawn points for half bullet
        foreach (Transform trans in GetComponentsInChildren<Transform>())
        {
            if (trans.name == "Spawn_1")
                this.spawnPoint1 = trans;
            else if (trans.name == "Spawn_2")
                this.spawnPoint2 = trans;
        }

        //Init color
        foreach (MeshRenderer mesh in transform.GetComponentsInChildren<MeshRenderer>())
        {
            if (this.id == 10)
                mesh.material = Resources.Load<Material>("Material/EggPlayer_1");
            else if (this.id == 20)
                mesh.material = Resources.Load<Material>("Material/EggPlayer_2");
        }
    }

    public void Launch(Transform trans)
    {
        //Add insane force Reeeeeeeee
        this.rb.velocity = trans.forward * -this.throwForce;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!hasBroken)
        {
            hasBroken = true;
            //Test if its a chiken
            switch (other.transform.tag)
            {
                //Check if its my egg
                case "10":
                    if (this.id == 20)
                    {
                        other.transform.GetComponent<PlayerEvents>().OnAction(PlayerEvents.Event.DIE);
                        PlayerManager.Instance.player2Events.SmackThatChick();
                    }

                    break;
                case "20":
                    if (this.id == 10)
                    {
                        other.transform.GetComponent<PlayerEvents>().OnAction(PlayerEvents.Event.DIE);
                        PlayerManager.Instance.player1Events.SmackThatChick();
                    }

                    break;
            }

            Explode();
            GameObject.Destroy(gameObject);
        }
    }

    private void Explode()
    {
        //Create 2 half bullet
        Rigidbody rb1 = GameObject.Instantiate(this.bulletTopPart, this.spawnPoint1.position, this.spawnPoint1.rotation)
            .GetComponent<Rigidbody>();
        Rigidbody rb2 = GameObject
            .Instantiate(this.bulletBottomPart, this.spawnPoint2.position, this.spawnPoint2.rotation)
            .GetComponent<Rigidbody>();
        rb1.GetComponent<Bullet_Half>().Initialize(this.id);
        rb2.GetComponent<Bullet_Half>().Initialize(this.id);

        //Add explosion force to them
        rb1.velocity = (-transform.forward * this.explosionForce) +
                       new Vector3(Random.Range(-10, 5), Random.Range(-5, 10), Random.Range(-7, 5));
        rb2.velocity = (-transform.forward * this.explosionForce) +
                       new Vector3(Random.Range(-5, 10), Random.Range(-3, 5), Random.Range(-5, 9));

        //   Vector3 explosionPosition = transform.position + this.offsetExplosion;
        //   explosionPosition += new Vector3(0, 0, 0);
        //   rb1.AddExplosionForce(this.explosionForce, explosionPosition, this.explosionRadius, 1,
        //       ForceMode.Impulse);
        //   rb2.AddExplosionForce(this.explosionForce, explosionPosition, this.explosionRadius, 1,
        //       ForceMode.Impulse);
    }
}