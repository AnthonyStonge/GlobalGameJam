using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPlayer : MonoBehaviour
{
    Rigidbody rb;
    public float upForce;
    public float rayLength;

    [Header("CHECK CA CRISS!")]
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent <Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += new Vector3(0, upForce, 0);
        }
        
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -gameObject.transform.up, out hit, 1f)){
            Debug.Log(hit.transform.gameObject.name);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Ray testRay = new Ray();
        testRay.origin = gameObject.transform.position;
        testRay.direction = -gameObject.transform.up;
        Gizmos.color = Color.cyan; 
        Gizmos.DrawRay(testRay);
    }

}
