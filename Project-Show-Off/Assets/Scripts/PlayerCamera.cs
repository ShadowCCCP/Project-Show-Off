using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Transform VRCamera;
    [SerializeField]
    Transform player;

    [SerializeField]
    bool falling;

    bool playerPhysics;

    float startY;

    void Awake()
    {
        EventBus<StopPlayerMovementEvent>.OnEvent += AllowPlayerPhysics;
    }

    void OnDestroy()
    {
        EventBus<StopPlayerMovementEvent>.OnEvent -= AllowPlayerPhysics;
    }

    void Start()
    {
        startY = player.position.y;
    }

    void Update()
    {
        if(player.transform.position.y < startY)
        {
            falling = true;
        }
        else
        {
            falling = false;
        }

        if (!falling)
        {
            transform.position = VRCamera.transform.position;
        }
        else
        {
            transform.position = player.transform.position;
            
        }
        
        transform.rotation = VRCamera.transform.rotation;

        if (!playerPhysics)
        {

            player.position = new Vector3(transform.position.x, player.position.y, transform.position.z);
        }
        else
        {
            Debug.Log("ja");
            falling = true;
        }

    }

    void AllowPlayerPhysics(StopPlayerMovementEvent stopPlayerMovementEvent)
    {
        playerPhysics = true;
        StartCoroutine(returnVrCameraControl());
    }

    IEnumerator returnVrCameraControl()
    {
        yield return new WaitForSeconds(1);
        playerPhysics = false;



    }
}
