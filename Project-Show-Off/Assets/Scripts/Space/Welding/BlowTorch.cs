using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowTorch : MonoBehaviour
{
    [SerializeField]
    ParticleSystem sparks;
    [SerializeField]
    ParticleSystem smoke;


    private void Start()
    {
        sparks.Stop();
        smoke.Stop();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<WeldableCube>() != null)
        {
            sparks.Play();
            smoke.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<WeldableCube>() != null)
        {
            sparks.Stop();
            smoke.Stop();
        }
    }
}
