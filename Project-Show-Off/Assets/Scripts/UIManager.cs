using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator blackImg;
    void Awake()
    {
        EventBus<DarkenScreenEvent>.OnEvent += SetScreenDark;
    }

    void OnDestroy()
    {
        EventBus<DarkenScreenEvent>.OnEvent -= SetScreenDark;
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
    }
}
