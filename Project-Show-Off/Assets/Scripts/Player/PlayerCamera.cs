using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Camera VRCamera;

    Camera playerCam;

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

    [SerializeField]
    Transform camOffset;

    [SerializeField]
    Transform origin;


    Vector3 offset = new Vector3 (0, 0, 0);

    bool playerCamActive = false;

    bool checkFallActive = true;

    bool canMoveRigidBody = false;
    void Awake()
    {
        EventBus<StopPlayerMovementEvent>.OnEvent += AllowPlayerPhysics;
        startY = player.position.y;
    }

    void OnDestroy()
    {
        EventBus<StopPlayerMovementEvent>.OnEvent -= AllowPlayerPhysics;
    }

    void Start()
    {
        playerCam = GetComponent<Camera>();
        playerCam.enabled = false;

        if (GetComponent<IntroPositionSwitch>())
        {
            canMoveRigidBody = false;
            checkFallActive = false;
        }
        else
        {
            canMoveRigidBody = true;
        }

        if (GameManager.Instance != null)
        {
            offset = GameManager.Instance.PositionBeforeReset;
        }
        else
        {
            Debug.Log("There is no game manager");
        }

        camOffset.position += offset;
 
        
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
        if (player.transform.position.y < startY && checkFallActive)
        {
            if (falling == false)
            {
                falling = true;

                StartCoroutine( beforeDeathTimer());
            }
        }


        if (!falling )
        {
            if (playerCamActive)
            {
                playerCam.enabled = false;
                VRCamera.enabled = true;
                playerCamActive = false;
            }
        }
        else
        {
            if (!playerCamActive)
            {
                playerCam.enabled = true;
                playerCamActive = true;
            }
            transform.position = player.transform.position + offset;
           
        }
    }

    void playerPhysicsCheck()
    {
        if (!playerPhysics)
        {
            if (canMoveRigidBody)
            {
                player.position = new Vector3(VRCamera.transform.position.x, player.position.y, VRCamera.transform.position.z);
            }
        }
        else
        {
            
            falling = true;
        }
    }

    void AllowPlayerPhysics(StopPlayerMovementEvent stopPlayerMovementEvent)
    {
        if (checkFallActive)
        {
            Debug.Log("Player physiscs activated");
            playerPhysics = true;
            StartCoroutine(beforeDeathTimer());
        }
    }

    IEnumerator beforeDeathTimer()
    {
        Debug.Log("You have fallen"); 
        rightController.SetActive(false);
        leftController.SetActive(false);
       
        EventBus<OnPlayerDeathEvent>.Publish(new OnPlayerDeathEvent(transform.position - origin.position, fallingTime));

        yield return new WaitForSeconds(1);
    }

    public void ActivateCheckFall(bool pCheckFallActive)
    {
        checkFallActive = pCheckFallActive;

        canMoveRigidBody = pCheckFallActive;
    }

}
