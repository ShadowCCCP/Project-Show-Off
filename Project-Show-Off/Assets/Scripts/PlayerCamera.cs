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

    [SerializeField]
    float fallingTime = 1;

    Vector3 offset = new Vector3 (0, 0, 0);

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
        offset = GameManager.Instance.PositionBeforeReset; 
        //player.position = offset;
        startY = player.position.y;
    }

    void Update()
    {

        //check for falling
        playerFallingCheck();

        //keep the correct rotation
        transform.rotation = VRCamera.transform.rotation;

        //allow or disallow player physics for space game 
        playerPhysicsCheck();

    }

    void playerFallingCheck()
    {
        if (player.transform.position.y < startY)
        {
            if (falling == false)
            {
                falling = true;
                doFallingBeforeDeath();
            }
        }
        else
        {
            falling = false;
        }

        if (!falling)
        {
            transform.position = VRCamera.transform.position + offset;
        }
        else
        {
            transform.position = player.transform.position + offset;

        }
    }
    void playerPhysicsCheck()
    {
        if (!playerPhysics)
        {

            player.position = new Vector3(transform.position.x, player.position.y, transform.position.z) ;
        }
        else
        {
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

    void doFallingBeforeDeath()
    {
        Debug.Log("You have fallen");
        EventBus<OnPlayerDeathEvent>.Publish(new OnPlayerDeathEvent(transform.position));
    }

}
