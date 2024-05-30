using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator blackImg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {

            blackImg.SetTrigger("DarkenScreen");
        }
    }


    void SetScreenDark()
    {
        blackImg.SetTrigger("DarkenScreen");
    }
}
