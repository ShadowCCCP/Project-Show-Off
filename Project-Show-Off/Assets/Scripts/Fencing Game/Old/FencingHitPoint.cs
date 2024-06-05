using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FencingHitPoint : MonoBehaviour
{
    [SerializeField] FencingEnemy enemyScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            enemyScript.HitPointHit(transform.gameObject);
        }
    }
}
