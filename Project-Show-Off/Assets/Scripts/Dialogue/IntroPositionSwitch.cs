using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

/// <summary>
/// Alllows the player start in a different place without breaking the gameplay
/// </summary>

public class IntroPositionSwitch : MonoBehaviour
{
    [SerializeField]
    Transform placeToGo;

    [SerializeField]
    Transform cameraOffset;
    [SerializeField]
    XROrigin origin;

    [SerializeField]
    GameObject player;

    [SerializeField]
    bool startInNewPlace = true;

    PlayerCamera playerCamera;

    Vector3 startPos;
    SoundPlayer playerSound;

    [SerializeField]
    float yOffset;

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
            origin.CameraYOffset += yOffset;
        }

    }

    void goBackToOriginalPlace(GoBackToStartPosEvent goBack)
    {
       
        cameraOffset.position = startPos; 
        origin.CameraYOffset -= yOffset;

        playerCamera.ActivateCheckFall(true);

        if(playerSound)
        playerSound.Play();
    }
}
