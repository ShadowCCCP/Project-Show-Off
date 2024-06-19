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
    void Update()
    {
        /*
        if (Input.GetKey(KeyCode.E))
        {

            blackImg.SetTrigger("DarkenScreen");
        }*/
    }


    void SetScreenDark(DarkenScreenEvent weldCubeEvent)
    {
        Debug.Log("trogg anim");
        blackImg.SetTrigger("DarkenScreen");
        if (soundPlayer) soundPlayer.PlaySpecific(1);
    }
}
