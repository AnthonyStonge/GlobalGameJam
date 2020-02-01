using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using Random = System.Random;

public class Bullet : MonoBehaviour
{
    public float explosionForce = 10;
    public float explosionRadius = 2;
    public Vector3 offsetExplosion;

    public GameObject bulletTopPart;
    public GameObject bulletBottomPart;

    private Transform spawnPoint1, spawnPoint2;

    private void Awake()
    {
        Initialize();
    }
    
    public void Initialize()
    {
        //Get spawn points for half bullet
        foreach (Transform trans in GetComponentsInChildren<Transform>())
        {
            if (trans.name == "Spawn_1")
                this.spawnPoint1 = trans;
            else if (trans.name == "Spawn_2")
                this.spawnPoint2 = trans;
        }
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

        //Add explosion force to them
        Vector3 explosionPosition = transform.position + this.offsetExplosion;
        explosionPosition += new Vector3(UnityEngine.Random.Range(0, 2), 0, UnityEngine.Random.Range(0, 2));
        rb1.AddExplosionForce(this.explosionForce, explosionPosition, this.explosionRadius, 1,
            ForceMode.Impulse);
        rb2.AddExplosionForce(this.explosionForce, explosionPosition, this.explosionRadius, 1,
            ForceMode.Impulse);
    }
}