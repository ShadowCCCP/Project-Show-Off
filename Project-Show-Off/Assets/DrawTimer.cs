using System.Collections;
using TMPro;
using UnityEngine;

public class DrawTimer : MonoBehaviour
{
    [SerializeField] int timerInSeconds = 120;
    private float _timePassed = 0;

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

        GameManager.Instance.LoadSceneNext();
    }
}
