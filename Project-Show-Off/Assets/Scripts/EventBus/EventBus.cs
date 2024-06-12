using System;
using UnityEngine;
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

public class LeverActivatedEvent : Event  
{
    public LeverActivatedEvent()
    {

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

