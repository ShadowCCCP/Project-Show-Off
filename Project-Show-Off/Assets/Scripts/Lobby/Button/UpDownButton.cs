using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownButton : VRAbstractButton
{
    [SerializeField]
    bool upButton = false;

    public override void OnButtonPress()
    {

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
