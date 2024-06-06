using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerSword : MonoBehaviour
{
    public static event Action onSwordHit;
    public static event Action onSwordGrabbed;

    private Rigidbody _rb;
    private XRGrabInteractable _gInteractable;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gInteractable = GetComponent<XRGrabInteractable>();

        _gInteractable.selectEntered.AddListener(SwordGrabbed);
    }

    private void OnDestroy()
    {
        _gInteractable.selectEntered.RemoveListener(SwordGrabbed);
    }

    public void Update()
    {
        ResetVelocity();
    }

    private void SwordGrabbed(SelectEnterEventArgs args)
    {
        Debug.Log("SwordGrabbed");
        if (onSwordGrabbed != null) { onSwordGrabbed(); }

        // Reset rigidbody constraints to make sword moveable...
        _rb.constraints = RigidbodyConstraints.None;
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
