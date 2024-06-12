using System;
using UnityEngine;

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
