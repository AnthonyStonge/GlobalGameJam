using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickJump : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(EnableJump());
    }

    // Update is called once per frame
    public IEnumerator EnableJump()
    {
        yield return new WaitForSeconds(Random.Range(0, 5f));
        anim.SetTrigger("Jump");
        
    }
}
