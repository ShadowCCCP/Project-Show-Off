using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    [SerializeField] float targetRotation = 180;
    [SerializeField] float secondsToPaint = 120;

    [SerializeField] float warnAtSeconds = 60;

    [SerializeField] float timeBeforeLevelFinish = 5;

    private SoundPlayer _soundPlayer;

    private bool _firstBellRang;
    private float _firstBellTime;
    private bool _secondBellRang;
    private float _secondBellTime;

    private float _totalRotation;
    private bool _warned;

    private bool _rotate;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();
        if (_soundPlayer == null)
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nSunRotation: No soundPlayer script attached to the gameObject.");
        }

        GetBellTimings();

        EventBus<PaintTellAboutList>.OnEvent += ActivateRotateScript;
    }

    private void OnDestroy()
    {
        EventBus<PaintTellAboutList>.OnEvent -= ActivateRotateScript;
    }

    void Update()
    {
        CheckWarnings();
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

                // Play final bell sound...
                PlayBellSound(2);
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

    private void GetBellTimings()
    {
        _firstBellTime = secondsToPaint / 3 * 2;
        _secondBellTime = secondsToPaint / 3;
    }

    private void CheckWarnings()
    {
        // Check the warning time...
        if (RemainingTime() <= warnAtSeconds && !_warned)
        {
            EventBus<PaintTimeRunningOutEvent>.Publish(new PaintTimeRunningOutEvent());
            _warned = true;
        }

        // Check the bell rings...
        if (RemainingTime() <= _firstBellTime && !_firstBellRang)
        {
            PlayBellSound(0);
        }
        else if (RemainingTime() <= _secondBellTime && !_secondBellRang)
        {
            PlayBellSound(1);
        }
    }

    private void PlayBellSound(int pIndex)
    {
        _soundPlayer.PlaySpecific(pIndex);
    }

    private void ActivateRotateScript(PaintTellAboutList paintTellAboutList)
    {
        _rotate = true;
    }
}
