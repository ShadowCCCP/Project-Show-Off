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
        EventBus<WeldCubeEvent>.OnEvent += SetScreenDark;
    }

    void OnDestroy()
    {
        EventBus<WeldCubeEvent>.OnEvent -= SetScreenDark;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {

            blackImg.SetTrigger("DarkenScreen");
        }
    }


    void SetScreenDark(WeldCubeEvent weldCubeEvent)
    {
        blackImg.SetTrigger("DarkenScreen");
    }
}
