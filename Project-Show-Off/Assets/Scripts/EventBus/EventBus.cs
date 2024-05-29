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

