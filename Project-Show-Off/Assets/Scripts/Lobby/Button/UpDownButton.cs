using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownButton : VRAbstractButton
{
    [SerializeField]
    bool upButton = false;

    SoundPlayer soundPlayer;

    private void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
    }
    public override void OnButtonPress()
    {
        soundPlayer.Play();
        if (upButton)
        {
            EventBus<MoveCrankEvent>.Publish(new MoveCrankEvent(true));
        }
        else
        {
            EventBus<MoveCrankEvent>.Publish(new MoveCrankEvent(false));
        }
    }
}
