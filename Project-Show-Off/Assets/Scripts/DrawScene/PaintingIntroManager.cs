using System;
using UnityEngine;

public class PaintingIntroManager : MonoBehaviour
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
        brushGrabbedOnce = true;
    }

    private void PalleteGrabbed()
    {
        palleteGrabbedOnce = true;
    }
}
