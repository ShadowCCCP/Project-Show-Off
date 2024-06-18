using System;
using UnityEngine;

public class DrawSequenceManager : MonoBehaviour
{
    public static event Action onBrushPickedUp;
    public static event Action onPalletePickedUp;

    public enum PaintUtils { Brush, Pallete }

    private bool brushGrabbedOnce;
    private bool palleteGrabbedOnce;

    private bool sentTransitionSignal;

    private void Start()
    {
        onBrushPickedUp += BrushGrabbed;
        onPalletePickedUp += PalleteGrabbed;
    }

    private void OnDestroy()
    {
        onBrushPickedUp -= BrushGrabbed;
        onPalletePickedUp -= PalleteGrabbed;
    }

    private void Update()
    {
        if (!sentTransitionSignal && brushGrabbedOnce && palleteGrabbedOnce)
        {
            sentTransitionSignal = true;
        }
    }

    private void BrushGrabbed()
    {
        if (!brushGrabbedOnce) { brushGrabbedOnce = true; }
    }

    private void PalleteGrabbed()
    {
        if (!palleteGrabbedOnce) { palleteGrabbedOnce = true; }
    }
}
