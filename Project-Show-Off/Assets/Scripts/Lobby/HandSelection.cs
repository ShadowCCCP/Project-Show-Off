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

    [SerializeField]
    TimeMachineManager timeMachineManager;
    [SerializeField]
    GameObject spotlight;

    bool brokenGlass;
    SoundPlayer soundPlayer;

    [SerializeField]
    SoundPlayer alarmSound;

    private void Awake()
    {
        EventBus<GlassBrokenEvent>.OnEvent += BreakGlass;
    }

    void OnDestroy()
    {
        EventBus<GlassBrokenEvent>.OnEvent -= BreakGlass;
    }
    private void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (brokenGlass)
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
        spotlight.SetActive(false);
        alarmSound.Stop();
    }

    void BreakGlass(GlassBrokenEvent glassBrokenEvent)
    {
        brokenGlass=true;
        spotlight.SetActive(true);
        soundPlayer.Play();
        alarmSound.Play();
    }
}
