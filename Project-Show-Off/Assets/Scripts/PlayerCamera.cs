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
    float fallingTime = 2;

    [SerializeField]
    GameObject rightController;
    [SerializeField]
    GameObject leftController;

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
        if (GameManager.Instance != null)
        {
            offset = GameManager.Instance.PositionBeforeReset;
        }
        else
        {
            Debug.Log("There is no game manager");
        }
 
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

                StartCoroutine( beforeDeathTimer());
            }
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
        Debug.Log("Player physiscs activated");
        playerPhysics = true; 
        StartCoroutine(beforeDeathTimer());
    }

    IEnumerator beforeDeathTimer()
    {
        Debug.Log("You have fallen"); 
        rightController.SetActive(false);
        leftController.SetActive(false);
        yield return new WaitForSeconds(fallingTime);

        EventBus<OnPlayerDeathEvent>.Publish(new OnPlayerDeathEvent(transform.position));

        
    }

}
