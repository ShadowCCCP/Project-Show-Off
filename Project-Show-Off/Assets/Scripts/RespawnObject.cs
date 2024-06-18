using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RespawnObject : MonoBehaviour
{
    [SerializeField] Vector3 respawnPosition;
    [SerializeField] float respawnTime;

    private XRGrabInteractable _gInteractable;
    private bool _active;

    private bool _activatedOnce;
    private bool _respawned;

    // Timer to check if to respawn...
    private float _currentTime;

    private void Start()
    {
        _gInteractable = GetComponent<XRGrabInteractable>();

        if (_gInteractable == null) 
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nRespawnObject: Script attached to an object with no XRGrabInteractableScript.");
            return;
        }

        _gInteractable.selectEntered.AddListener(Activate);
    }

    private void OnDestroy()
    {
        _gInteractable.selectExited.AddListener(Deactivate);
    }

    private void Update()
    {
        if (!_active && _activatedOnce && !_respawned) 
        { 
            _currentTime += Time.deltaTime; 

            // If time is up...
            if (_currentTime >= respawnTime)
            {
                // Reset rotation and set back to the respawn position...
                transform.rotation = Quaternion.identity;
                transform.position = respawnPosition;

                _respawned = true;
            }
        }
    }

    private void ToggleTimer()
    {
        _active = !_active;
    }

    // For the grab events...
    private void Activate(SelectEnterEventArgs args)
    {
        if (!_activatedOnce) { _activatedOnce = true; }

        ToggleTimer();
    }

    private void Deactivate(SelectExitEventArgs args)
    {
        ToggleTimer();
    }
}
