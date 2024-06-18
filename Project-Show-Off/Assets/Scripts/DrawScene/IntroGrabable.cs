using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class IntroGrabable : MonoBehaviour
{
    public event Action onGrabbed;

    private XRGrabInteractable _gInteractable;

    private void Start()
    {
        _gInteractable = GetComponent<XRGrabInteractable>();

        if (_gInteractable == null)
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nIntroGrabable: Script attached to object with no XRGrabInteractable.");
        }

        _gInteractable.selectEntered.AddListener(NotifyManager);
    }

    private void TriggerOnGrabbed(SelectEnterEventArgs args)
    {
        if (_drawManager != null) { _drawManager.ObjectGrabbed(type); }
    }
}