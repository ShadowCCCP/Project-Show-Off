using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePhysics : MonoBehaviour
{
    [SerializeField]
    Collider player;

    [SerializeField]
    Collider[] ignoreCollisionWithPlayer;

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
        }   
     }
}