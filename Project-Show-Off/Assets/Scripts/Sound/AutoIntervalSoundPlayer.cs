using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntervalSoundPlayer : SoundPlayer
{
    [Tooltip("Decides how much minimum time in seconds needs to pass before a random sound starts playing.")]
    [SerializeField] float rangeFrom;
    [Tooltip("Decides how much maximum time in seconds can pass before a random sound starts playing")]
    [SerializeField] float rangeTo;

    private float _maxTime;
    private float _currentTime;

    private void Start()
    {
        if (ClipCount() <= 0)
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nIntervalSoundPlayer: No sound attached to the script.");
        }
        SetNewMaxTime();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _currentTime += Time.deltaTime;

        if (_currentTime >= _maxTime)
        {
            if (ClipCount() > 0)
            {
                PlayRandom();
                SetNewMaxTime();
                _currentTime = 0;
            }
        }
    }

    private void SetNewMaxTime()
    {
        _maxTime = Random.Range(rangeFrom, rangeTo);
    }
}
