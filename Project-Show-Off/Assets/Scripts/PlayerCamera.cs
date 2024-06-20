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
        playerCam = GetComponent<Camera>();
        playerCam.enabled = false;
        //transition.SetParent(VRCamera.transform);

        if (GameManager.Instance != null)
        {
            offset = GameManager.Instance.PositionBeforeReset;
        }
        else
        {
            Debug.Log("There is no game manager");
        }

        camOffset.position += offset;
 
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


        //if offeset playercaemra
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
                //transition.SetParent(VRCamera.transform);
                playerCamActive = false;
            }
            //do vr camera
            // transform.position = VRCamera.transform.position + offset;
        }
        else
        {
            if (!playerCamActive)
            {
                playerCam.enabled = true;

                //transition.SetParent(this.transform);
                playerCamActive = true;
            }
            transform.position = player.transform.position + offset;
           
        }
    }

    void playerPhysicsCheck()
    {
        if (!playerPhysics)
        {

            player.position = new Vector3(VRCamera.transform.position.x, player.position.y, VRCamera.transform.position.z) ;
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
        yield return new WaitForSeconds(fallingTime);
        EventBus<OnPlayerDeathEvent>.Publish(new OnPlayerDeathEvent(transform.position - origin.position));

        
    }

    public void ActivateCheckFall(bool pCheckFallActive)
    {
        checkFallActive = pCheckFallActive;
    }

}
