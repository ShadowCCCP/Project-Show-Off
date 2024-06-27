using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Makes the player be able to always float in space and not just sit in place when it falls off.
/// Prevents the player from colliding with everything that has a collider.
/// </summary>

public class SpacePhysics : MonoBehaviour
{
    [SerializeField]
    Collider player;

    [SerializeField]
    float glideSpeedOnFall = 0.1f;

    [SerializeField]
    Collider[] ignoreCollisionWithPlayer;

    [SerializeField]
    GameObject emergencyButton;

    void Start()
    {
        foreach (Collider p in ignoreCollisionWithPlayer)
        {
            Physics.IgnoreCollision(player, p);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            EventBus<StopPlayerMovementEvent>.Publish(new StopPlayerMovementEvent());
            

            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.AddForce(player.transform.forward * glideSpeedOnFall);

            emergencyButton.SetActive(false);
        }   
     }
}
