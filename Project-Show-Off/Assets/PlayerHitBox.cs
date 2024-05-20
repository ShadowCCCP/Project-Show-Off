using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    [SerializeField] FencingEnemy fencingEnemy;
    private bool _inZone;

    [SerializeField] float zoneTimeNeeded = 2.5f;
    private float _timeSpentInZone = 0.0f;

    private bool _initialized;

    private void Update()
    {
        if (_inZone) _timeSpentInZone += Time.deltaTime;

        if (!_initialized && _timeSpentInZone >= zoneTimeNeeded)
        {
            fencingEnemy.StartIntro();
            _initialized = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnterArea"))
        {
            _inZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("EnterArea"))
        {
            _inZone = false;
        }
    }
}
