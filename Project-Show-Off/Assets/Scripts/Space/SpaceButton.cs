using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpaceButton : VRAbstractButton
{
    [SerializeField]
    Animator door;

    [SerializeField]
    SoundPlayer doorSound;


    public override void OnButtonPress()
    {
        //open door
        door.SetTrigger("OpenDoor");
        doorSound.Play();

        EventBus<OnDoorOpenSpaceEvent>.Publish(new OnDoorOpenSpaceEvent());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            door.SetTrigger("OpenDoor");

            EventBus<OnDoorOpenSpaceEvent>.Publish(new OnDoorOpenSpaceEvent());
        }
    }

}
