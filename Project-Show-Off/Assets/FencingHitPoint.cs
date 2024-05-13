using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencingHitPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            gameObject.SetActive(false);
        }
    }
}
