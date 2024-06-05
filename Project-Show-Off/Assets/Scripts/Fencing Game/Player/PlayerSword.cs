using System;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public static event Action onSwordHit;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        ResetVelocity();
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
