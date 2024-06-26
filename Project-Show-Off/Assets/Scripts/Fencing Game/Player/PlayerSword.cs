using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSword : MonoBehaviour
{
    /// <summary>
    /// Indexes of all the sounds to be played:
    /// [ 0 - 1  ]  |   Swoosh (Sword swing)
    /// [ 2 - 8  ]  |   Sword clash
    /// </summary>

    [SerializeField] GameObject hitSparks;

    public static event Action onSwordHit;

    private Rigidbody _rb;
    private XRGrabInteractable _gInteractable;

    // To capture the swords velocity...
    private Vector3 _lastPosition;
    private bool _swingSoundInvalid;

    // To instantiate sword spark...
    List<ContactPoint> contactPoints = new List<ContactPoint>();

    private SoundPlayer _soundPlayer;

    private bool _swordGrabbedOnce;

    private float _sparksCooldown = 1.0f;
    private float _sparksCurrentTime;

    private float _swingCooldown = 1.0f;
    private float _swingCurrentTime;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();

        _rb = GetComponent<Rigidbody>();
        _gInteractable = GetComponent<XRGrabInteractable>();

        _gInteractable.selectEntered.AddListener(SwordGrabbed);
        _lastPosition = transform.position;
    }

    private void OnDestroy()
    {
        _gInteractable.selectEntered.RemoveListener(SwordGrabbed);
    }

    public void Update()
    {
        ResetVelocity();
        UpdateTimer();
    }

    private void FixedUpdate()
    {
        CheckSwing();
    }

    private void UpdateTimer()
    {
        _sparksCurrentTime += Time.deltaTime;
        _swingCurrentTime += Time.deltaTime;
    }

    private void CheckSwing()
    {
        // Makes the numbers better to work with...
        float distanceMoved = (transform.position - _lastPosition).magnitude * 100;

        if (distanceMoved >= 5 && !_swingSoundInvalid)
        {
            // Play sound once if the crossed distance reaches threshhold...
            _soundPlayer.PlayRandom(0, 1);
            _swingSoundInvalid = true;
            _swingCurrentTime = 0;
        }
        else if (_swingSoundInvalid && distanceMoved < 0.7f && _swingCurrentTime >= _swingCooldown)
        {
            // Reset bool to activate playing sound again, when lower threshhold is reached...
            _swingSoundInvalid = false;
        }

        _lastPosition = transform.position;
    }

    private void SwordGrabbed(SelectEnterEventArgs args)
    {
        if (!_swordGrabbedOnce)
        {
            StartCoroutine(TriggerGrabbedEvent());
            _swordGrabbedOnce = true;
        }
    }

    private IEnumerator TriggerGrabbedEvent()
    {
        yield return new WaitForSeconds(3);
        EventBus<OnSwordPickupEvent>.Publish(new OnSwordPickupEvent());
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
            _soundPlayer.PlayRandom(2, 8);
        }
        else if (collision.collider.CompareTag("Pirate"))
        {
            PirateBodyHitbox body = collision.collider.GetComponent<PirateBodyHitbox>();
            if (body != null)
            {
                body.SideHit();
            }
        }

        collision.GetContacts(contactPoints);
        if (_sparksCurrentTime >= _sparksCooldown &&
            contactPoints.Count > 0 && contactPoints[0].otherCollider.CompareTag("Sword"))
        {
            // Instantiate sparks...
            GameObject gawk = Instantiate(hitSparks, contactPoints[0].point, Quaternion.identity);
            gawk.transform.parent = null;

            // Reset timer...
            _sparksCurrentTime = 0;
        }
    }
}
