using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// First version of the timer used to end the level in the painting scene.
/// SunRotation script takes care of that now, so it's not used anymore.
/// </summary>

public class DrawTimer : MonoBehaviour
{
    [SerializeField] int timerInSeconds = 120;
    private float _timePassed = 0;

    [SerializeField] bool deactivateSceneSwtich;
    [SerializeField] float sceneSwitchTransitionTime = 4.0f;

    [SerializeField] TextMeshPro timerText;

    private void Start()
    {
        timerText.text = timerInSeconds.ToString();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        // Increase timer...
        if (_timePassed < timerInSeconds)
        {
            _timePassed += Time.deltaTime;

            // Update timer text...
            int timeLeft = timerInSeconds - (int)_timePassed;
            timerText.text = $"{timeLeft:D3}";

            // If time is up, start transition to next scene...
            if (_timePassed >= timerInSeconds)
            {
                StartCoroutine(FinishDrawing());
            }
        }
    }

    private IEnumerator FinishDrawing()
    {
        yield return new WaitForSeconds(sceneSwitchTransitionTime);

        if (!deactivateSceneSwtich) { GameManager.Instance.LoadSceneSpecific(4, true); }
    }
}
