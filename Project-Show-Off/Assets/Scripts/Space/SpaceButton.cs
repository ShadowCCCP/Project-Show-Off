using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Button in the space game
/// Can open door
/// </summary>
public class SpaceButton : VRAbstractButton
{
    [SerializeField]
    Animator door;

    [SerializeField]
    SoundPlayer doorSound;

    private void Awake()
    {
        EventBus<CloseSpaceDoorEvent>.OnEvent += CloseDoor;
    }

    void OnDestroy()
    {
        EventBus<CloseSpaceDoorEvent>.OnEvent -= CloseDoor;
    }
    public override void OnButtonPress()
    {
        openDoor();
       
    }

    void openDoor()
    {
        door.SetTrigger("OpenDoor");
        doorSound.Play();

        EventBus<OnDoorOpenSpaceEvent>.Publish(new OnDoorOpenSpaceEvent());
    }

    public void CloseDoor(CloseSpaceDoorEvent closeSpaceDoor)
    {
        door.SetTrigger("CloseDoor");
        doorSound.Play();
    }

}
