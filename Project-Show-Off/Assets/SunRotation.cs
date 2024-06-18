using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    [SerializeField] float targetRotation = 180;
    [SerializeField] float secondsToPaint = 120;

    [SerializeField] float warnAtSeconds = 60;

    private float _totalRotation;
    private bool _warned;

    void Update()
    {
        CheckWarning();
        Rotate();
    }

    private void Rotate()
    {
        float rotation = targetRotation / secondsToPaint * Time.deltaTime;
        gameObject.transform.Rotate(0, rotation, 0);
        _totalRotation += rotation;

        if (_totalRotation >= targetRotation)
        {
            EventBus<PaintDoneEvent>.Publish(new PaintDoneEvent());
            enabled = false;
        }
    }

    private float RemainingTime()
    {
        float currentRotation = Mathf.Clamp(_totalRotation, 0, targetRotation);
        float proportion = currentRotation / targetRotation;

        float remainingTime = secondsToPaint * (1 - proportion);
        return remainingTime;
    }

    private void CheckWarning()
    {
        if (RemainingTime() <= warnAtSeconds && !_warned)
        {
            EventBus<PaintTimeRunningOutEvent>.Publish(new PaintTimeRunningOutEvent());
            _warned = true;
        }
    }
}
