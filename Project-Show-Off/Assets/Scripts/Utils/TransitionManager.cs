using System;
using UnityEngine;

/// <summary>
/// Triggers static events through events triggered by the animator.
/// This helps to keep track of the current state of the UI transition for the player.
/// </summary>

public class TransitionManager : MonoBehaviour
{
    public static event Action onDarkenFinished;
    public static event Action onLightenFinished;

    public void FinishedDarken()
    {
        if (onDarkenFinished != null) { onDarkenFinished(); }
    }

    public void FinishedLigthen()
    {
        if (onLightenFinished != null) { onLightenFinished(); }
    }
}
