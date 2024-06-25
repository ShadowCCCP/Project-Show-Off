using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public void DestroyAnimator()
    {
        Animator anim = GetComponent<Animator>();
        anim.enabled = false;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }
}
