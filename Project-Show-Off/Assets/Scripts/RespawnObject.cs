using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RespawnObject : MonoBehaviour
{
    [SerializeField] bool debugMode;

    [SerializeField] Vector3 respawnPosition;
    [SerializeField] float respawnTime = 3.0f;

    private XRGrabInteractable _gInteractable;
    private Rigidbody _rb;
    private Floaty _floaty;

    private bool _active;

    private bool _activatedOnce;
    private bool _respawned;

    Quaternion _startRotation;

    // Timer to check if to respawn...
    private float _currentTime;

    private void Start()
    {
        _startRotation = transform.rotation;

        _gInteractable = GetComponent<XRGrabInteractable>();
        _rb = GetComponent<Rigidbody>(); 
        _floaty = GetComponent<Floaty>();

        if (_gInteractable == null) 
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nRespawnObject: Script attached to an object with no XRGrabInteractableScript.");
            return;
        }

        if (_rb != null && _rb.constraints != RigidbodyConstraints.FreezeAll)
        {
            _rb.constraints = RigidbodyConstraints.FreezeAll;
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
                Respawn();
            }
        }

        if (debugMode) { Testing(); }

        RotationFix();
    }

    private void RotationFix()
    {
        // This fixes the rotation as soon as the object is respawned...
        // No idea why it resets it's rotation in the first place, but it is what it is...
        if (_respawned && transform.rotation != _startRotation)
        {
            transform.rotation = _startRotation;
        }
    }

    private void Testing()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Activate(new SelectEnterEventArgs());
            _floaty.CancelTween(new SelectEnterEventArgs());
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Deactivate(new SelectExitEventArgs());
        }
    }

    private void Respawn()
    {
        // Reset rotation and set back to the respawn position...
        transform.rotation = _startRotation;
        transform.position = respawnPosition;
        _respawned = true;

        // Reset rigidbody values, if object has one...
        if (_rb != null)
        {
            _rb.angularVelocity = Vector3.zero;
            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            _rb.useGravity = false;
        }

        // Make object float again after respawn...
        if (_floaty != null) { _floaty.StartTween(); }
    }

    private void ToggleState()
    {
        _active = !_active;
    }

    // For the grab events...
    private void Activate(SelectEnterEventArgs args)
    {
        if (!_activatedOnce) { _activatedOnce = true; }
        if (_rb != null) { _rb.constraints = RigidbodyConstraints.None; }

        // Reset values...
        _currentTime = 0;
        _respawned = false;

        ToggleState();
    }

    private void Deactivate(SelectExitEventArgs args)
    {
        if (_rb != null) { _rb.useGravity = true; }
        ToggleState();
    }
}
