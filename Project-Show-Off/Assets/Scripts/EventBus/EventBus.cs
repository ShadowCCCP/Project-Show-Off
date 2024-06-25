using System;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public abstract class Event { }

//The event bus 
public class EventBus<T> where T : Event
{
    public static event Action<T> OnEvent;

    public static void Publish(T pEvent)
    {
        OnEvent?.Invoke(pEvent);
    }
}

public class GlassBrokenEvent : Event
{
    public GlassBrokenEvent()
    {

    }
}

public class MoveCrankEvent : Event
{
    public MoveCrankEvent(bool pUp)
    {
        up = pUp;
    }
    public bool up;
}

public class SetUpWeldableSettingsEvent : Event
{
    public SetUpWeldableSettingsEvent(float pWeldTime)
    {
        weldTime = pWeldTime;
    }
    public float weldTime;
}

public class WeldCubeEvent : Event
{
    public WeldCubeEvent(WeldableCube pCube)
    {
        cube = pCube;
    }
    public WeldableCube cube;
}

public class DarkenScreenEvent : Event
{
    public DarkenScreenEvent()
    {

    }
}


public class StopPlayerMovementEvent : Event
{
    public StopPlayerMovementEvent()
    {

    }
}


public class OnPlayerDeathEvent : Event
{
    public OnPlayerDeathEvent(Vector3 pPosDeath)
    {
        posDeath = pPosDeath;
    }

    public Vector3 posDeath;
}

public class SetPositionOffsetEvent : Event
{
    public SetPositionOffsetEvent(Vector3 posOffset)
    {
        this.posOffset = posOffset;
    }
    public Vector3 posOffset;
}

public class SpawnWeldablesEvent : Event
{
    public SpawnWeldablesEvent(MetalPlaceHolder metalPlaceHolder)
    {
        this.metalPlaceHolder = metalPlaceHolder;
    }
    public MetalPlaceHolder metalPlaceHolder;
}

public class ChangeHandEvent : Event
{
    public ChangeHandEvent()
    {

    }
}

public class OnLevelSelectedOnTMEvent : Event
{
    public OnLevelSelectedOnTMEvent()
    {

    }
}

public class OnSwordPickupEvent : Event
{
    public OnSwordPickupEvent()
    {

    }
}

public class OnPlayerHitEvent : Event
{
    public OnPlayerHitEvent()
    {

    }
}

public class OnPirateDefeatedEvent : Event
{
    public OnPirateDefeatedEvent()
    {

    }
}

public class OnPencilPickupEvent : Event
{
    public OnPencilPickupEvent()
    {

    }
}

public class PaintTellAboutList : Event
{
    public PaintTellAboutList()
    {

    }
}

public class PaintTimeRunningOutEvent : Event
{
    public PaintTimeRunningOutEvent()
    {

    }
}


public class PaintDoneEvent : Event
{
    public PaintDoneEvent()
    {

    }
}

public class GoUpScaffolding : Event
{
    public GoUpScaffolding()
    {

    }
}


public class LevelFinishedEvent : Event
{
    public LevelFinishedEvent(float pWaitTime)
    {
        waitTime = pWaitTime;
    }
    public float waitTime;
}

public class OnDoorOpenSpaceEvent : Event
{
    public OnDoorOpenSpaceEvent()
    {

    }
}

public class OnPlatePlacedSpaceEvent : Event
{
    public OnPlatePlacedSpaceEvent()
    {

    }
}

public class GoBackToStartPosEvent : Event
{
    public GoBackToStartPosEvent()
    {

    }
}

public class CloseSpaceDoorEvent : Event
{
    public CloseSpaceDoorEvent()
    {

    }
}
