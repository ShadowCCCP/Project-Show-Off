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
    bool startInNewPlace = true;

    PlayerCamera playerCamera;

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
        playerCamera = GetComponent<PlayerCamera>();
        if (startInNewPlace)
        {
            playerCamera.ActivateCheckFall(false);
            cameraOffset.position += placeToGo.position- origin.position;
        }

    }

    void goBackToOriginalPlace(GoBackToStartPosEvent goBack)
    {
        playerCamera.ActivateCheckFall(true);
        cameraOffset.position -= placeToGo.position + origin.position;
    }
}
