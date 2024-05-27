using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSelection : MonoBehaviour
{

    [SerializeField]
    Renderer leftLightRend;
    [SerializeField]
    Renderer rightLightRend;

    [SerializeField]
    Material materialOn;
    [SerializeField]
    Material materialOff;


    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "RightController"))
        {
            GameManager.Instance.SetDominantHand(1);
            updateLight(1);
        }
        if (other.tag == "LeftController")
        {
            GameManager.Instance.SetDominantHand(0);
            updateLight(0);
        }
    }

    void updateLight(int hand)
    {
        if(hand == 0)
        {
            leftLightRend.material = materialOn;
            rightLightRend.material = materialOff;
        }
        else 
        {
            leftLightRend.material = materialOff;
            rightLightRend.material = materialOn;
        }
    }
}
