using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] GameObject hitSparks;

    // To save start position...
    private Vector3 _startPos;
    private Quaternion _startRot;

    // Reset timer for sword...
    [Tooltip("Time that has to pass without the player holding it, before it resets.")]
    [SerializeField] float timeUntilReset = 2.5f;
    private float _resetTimePassed;
    private bool _resetTimerActive;
    

    public static event Action onSwordHit;
    public static event Action onSwordGrabbed;

    private Rigidbody _rb;
    private XRGrabInteractable _gInteractable;

    // To capture the swords velocity...
    private Vector3 _lastPosition;
    private bool _swingSoundInvalid;

    // To instantiate sword spark...
    List<ContactPoint> contactPoints = new List<ContactPoint>();

    private SoundPlayer _soundPlayer;

    private void Start()
    {
        _startPos = transform.position;
        _startRot = transform.rotation;

        _soundPlayer = GetComponent<SoundPlayer>();

        _rb = GetComponent<Rigidbody>();
        _gInteractable = GetComponent<XRGrabInteractable>();

        _gInteractable.selectEntered.AddListener(SwordGrabbed);
        _gInteractable.selectExited.AddListener(SwordLost);
        _lastPosition = transform.position;
    }

    private void OnDestroy()
    {
        _gInteractable.selectEntered.RemoveListener(SwordGrabbed);
        _gInteractable.selectExited.RemoveListener(SwordLost);
    }

    public void Update()
    {
        ResetVelocity();
        UpdateResetTimer();
    }

    private void FixedUpdate()
    {
        CheckSwing();
    }

    private void UpdateResetTimer()
    {
        if (_resetTimerActive)
        {
            _resetTimePassed += Time.deltaTime;

            if (_resetTimePassed >= timeUntilReset)
            {
                ResetPosition();
                _resetTimerActive = false;
            }
        }
    }

    private void ResetPosition()
    {
        Debug.Log("Reset");
        transform.position = _startPos;
        transform.rotation = _startRot;

        _rb.constraints = RigidbodyConstraints.FreezeAll;
        _rb.useGravity = false;
    }

    private void CheckSwing()
    {
        float distanceMoved = (transform.position - _lastPosition).magnitude * 100;

        if (distanceMoved >= 5 && !_swingSoundInvalid)
        {
            // Play sound once if the crossed distance reaches threshhold...
            _soundPlayer.PlayRandom();
            _swingSoundInvalid = true;
        }
        else if (_swingSoundInvalid && distanceMoved < 0.7f)
        {
            // Reset bool to activate playing sound again, when lower threshhold is reached...
            _swingSoundInvalid = false;
        }

        _lastPosition = transform.position;
    }

    private void SwordGrabbed(SelectEnterEventArgs args)
    {
        if (onSwordGrabbed != null) { onSwordGrabbed(); }

        // Reset timer...
        _resetTimerActive = false;
        _resetTimePassed = 0;

        // Reset rigidbody constraints to make sword moveable...
        _rb.constraints = RigidbodyConstraints.None;
        _rb.useGravity = false;
    }

    private void SwordLost(SelectExitEventArgs args)
    {
        _resetTimerActive = true;
        _rb.useGravity = true;
    }

    private void ResetVelocity()
    {
        // Reset the angular velocity to zero...
        if (_rb != null && _rb.angularVelocity != Vector3.zero)
        {
            _rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Sword"))
        {
            if (onSwordHit != null) { onSwordHit(); }
        }
        // REMOVE LATER:
        else if (collision.collider.CompareTag("Pirate"))
        {
            PirateBodyHitbox body = collision.collider.GetComponent<PirateBodyHitbox>();
            if (body != null)
            {
                body.SideHit();
            }
        }

        collision.GetContacts(contactPoints);
        if (contactPoints.Count > 0 && contactPoints[0].otherCollider.CompareTag("Sword"))
        {
            GameObject gawk = Instantiate(hitSparks, contactPoints[0].point, Quaternion.identity);
            gawk.transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pirate"))
        {
            PirateBodyHitbox body = other.GetComponent<PirateBodyHitbox>();
            if (body != null) 
            {
                body.SideHit();
            }
        }
    }
}
