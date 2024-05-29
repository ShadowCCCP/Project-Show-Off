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


public class EnemyDeathEvent : Event //enemy died event 
{
    public EnemyDeathEvent(int pEnemy)
    {
        enemy = pEnemy;
    }
    public int enemy;
}