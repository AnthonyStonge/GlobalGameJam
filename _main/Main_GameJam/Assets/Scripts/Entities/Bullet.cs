using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using Random = System.Random;

public class Bullet : MonoBehaviour
{
    public float throwForce = 200;
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
    

    public void Initialize(int id)
    {
        this.id = id;
        
        this.rb = transform.GetComponent<Rigidbody>();
        this.colliders = transform.GetComponentsInChildren<SphereCollider>();
        
        //Get spawn points for half bullet
        foreach (Transform trans in GetComponentsInChildren<Transform>())
        {
            if (trans.name == "Spawn_1")
                this.spawnPoint1 = trans;
            else if (trans.name == "Spawn_2")
                this.spawnPoint2 = trans;
        }
    }

    public void Launch(Transform trans)
    {
        //Add insane force Reeeeeeeee
        this.rb.velocity = trans.forward * -this.throwForce;
    }

    private void OnCollisionEnter(Collision other)
    {
        Explode();
        GameObject.Destroy(gameObject);
    }

    private void Explode()
    {
        //Create 2 half bullet
        Rigidbody rb1 = GameObject.Instantiate(this.bulletTopPart, this.spawnPoint1.position, this.spawnPoint1.rotation)
            .GetComponent<Rigidbody>();
        Rigidbody rb2 = GameObject.Instantiate(this.bulletBottomPart, this.spawnPoint2.position, this.spawnPoint2.rotation)
            .GetComponent<Rigidbody>();
        rb1.GetComponent<Bullet_Half>().Initialize(this.id);
        rb2.GetComponent<Bullet_Half>().Initialize(this.id);

        //Add explosion force to them
        Vector3 explosionPosition = transform.position + this.offsetExplosion;
        explosionPosition += new Vector3(0, 0, 0);
        rb1.AddExplosionForce(this.explosionForce, explosionPosition, this.explosionRadius, 1,
            ForceMode.Impulse);
        rb2.AddExplosionForce(this.explosionForce, explosionPosition, this.explosionRadius, 1,
            ForceMode.Impulse);
    }
}