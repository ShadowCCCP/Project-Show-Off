using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencingIntro : MonoBehaviour
{
    [SerializeField] GameObject piratePrefab;
    [SerializeField] Vector3 pirateSpawnPos;
    [SerializeField] Quaternion pirateRotation;

    private bool _startTimer;
    private float _timerCooldown = 3;
    private float _currentTime;

    private void Start()
    {
        EventBus<OnSwordPickupEvent>.OnEvent += QueuePirateSpawn;
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void QueuePirateSpawn(OnSwordPickupEvent pOnSwordPickupEvent)
    {
        _startTimer = true;
    }

    private void UpdateTimer()
    {
        if (_startTimer)
        {
            _currentTime += Time.deltaTime;

            if (_currentTime >= _timerCooldown)
            {
                Instantiate(piratePrefab, pirateSpawnPos, pirateRotation, null);
                _startTimer = false;
            }
        }
    }
}
