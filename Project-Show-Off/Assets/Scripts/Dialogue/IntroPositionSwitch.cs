using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPositionSwitch : MonoBehaviour
{
    [SerializeField]
    Transform placeToGo;

    [SerializeField]
    Transform cameraOffset;
    [SerializeField]
    Transform origin;

    [SerializeField]
    GameObject player;

    [SerializeField]
    bool startInNewPlace = true;

    PlayerCamera playerCamera;

    Vector3 startPos;
    SoundPlayer playerSound;

    private void Awake()
    {
        EventBus<GoBackToStartPosEvent>.OnEvent += goBackToOriginalPlace;
    }

    void OnDestroy()
    {
        EventBus<GoBackToStartPosEvent>.OnEvent -= goBackToOriginalPlace;
    }
    void Start()
    {
        startPos = transform.position;
        playerCamera = GetComponent<PlayerCamera>();
        playerSound = GetComponent<SoundPlayer>();
        if (startInNewPlace)
        {
            playerCamera.ActivateCheckFall(false);
            cameraOffset.position = placeToGo.position;
        }

    }

    void goBackToOriginalPlace(GoBackToStartPosEvent goBack)
    {
       
        cameraOffset.position = startPos ;

        //player.transform.position = startPos ;
       // player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        playerCamera.ActivateCheckFall(true);

        if(playerSound)
        playerSound.Play();
    }
}
