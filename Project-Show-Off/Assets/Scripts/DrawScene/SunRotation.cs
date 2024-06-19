using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    [SerializeField] float targetRotation = 180;
    [SerializeField] float secondsToPaint = 120;

    [SerializeField] float warnAtSeconds = 60;

    [SerializeField] float timeBeforeLevelFinish = 5;

    private float _totalRotation;
    private bool _warned;

    private bool _rotate;

    private void Start()
    {
        EventBus<PaintTellAboutList>.OnEvent += ActivateRotateScript;
    }

    private void OnDestroy()
    {
        EventBus<PaintTellAboutList>.OnEvent -= ActivateRotateScript;
    }

    void Update()
    {
        CheckWarning();
        if (_rotate) { Rotate(); }
    }

    private void Rotate()
    {
        if (_totalRotation < targetRotation)
        {
            float rotation = targetRotation / secondsToPaint * Time.deltaTime;
            gameObject.transform.Rotate(0, rotation, 0);
            _totalRotation += rotation;

            if (_totalRotation >= targetRotation)
            {
                EventBus<PaintDoneEvent>.Publish(new PaintDoneEvent());
                StartCoroutine(FinishLevel());
            }
        }
    }

    private IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(timeBeforeLevelFinish);
        EventBus<LevelFinishedEvent>.Publish(new LevelFinishedEvent());
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

    private void ActivateRotateScript(PaintTellAboutList paintTellAboutList)
    {
        _rotate = true;
    }
}
