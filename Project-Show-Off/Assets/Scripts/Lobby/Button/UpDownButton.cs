using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Can move the lever on time machine 
/// 
/// Mechanic added back when using a lever was thpugh to be too complicated
/// </summary>
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
