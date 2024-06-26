using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator blackImg;

    SoundPlayer soundPlayer;
    void Awake()
    {
        EventBus<DarkenScreenEvent>.OnEvent += SetScreenDark;
    }

    void OnDestroy()
    {
        EventBus<DarkenScreenEvent>.OnEvent -= SetScreenDark;
    }

    private void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();

        if (soundPlayer) soundPlayer.PlaySpecific(0);
    }


    void SetScreenDark(DarkenScreenEvent weldCubeEvent)
    {
        Debug.Log("triggered anim transitions");
        blackImg.SetTrigger("DarkenScreen");
        if (soundPlayer) soundPlayer.PlaySpecific(1);
    }
}
