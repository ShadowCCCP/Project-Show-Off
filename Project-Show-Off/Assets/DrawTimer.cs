using UnityEngine;

public class DrawTimer : MonoBehaviour
{
    [SerializeField] float totalTimeInSeconds = 120;
    private float _timePassed = 0;

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        _timePassed += Time.deltaTime;

        if (_timePassed >= totalTimeInSeconds)
        {
            FinishDrawing();
        }
    }

    private void FinishDrawing()
    {

    }
}
