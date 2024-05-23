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
/*
    void Awake()
    {
    EventBus<EnemyDeathEvent>.OnEvent += EnemyDeath;
    EventBus<AddEnemyToEnemyListEvent>.OnEvent += addEnemy;
}

void OnDestroy()
{
    EventBus<EnemyDeathEvent>.OnEvent -= EnemyDeath;
    EventBus<AddEnemyToEnemyListEvent>.OnEvent -= addEnemy;
}
*/

//EventBus<StartNextWaveEvent>.Publish(new StartNextWaveEvent());
public class StartNextWaveEvent : Event //next wave event 
{
    public StartNextWaveEvent()
    {

    }
}

public class EnemyDeathEvent : Event //enemy died event 
{
    public EnemyDeathEvent(int pEnemy)
    {
        enemy = pEnemy;
    }
    public int enemy;
}