using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.XR.CoreUtils;
using UnityEngine;

public class FencingEnemy : MonoBehaviour
{
    // This event is triggered when the player gets on the plank...
    public event Action onTriggerFight;
    // This event is triggered as soon as a certain animation is finished...
    public event Action onNextSequence;

    [SerializeField] int attackCount;
    private int _currentStage;

    private List<GameObject> _hitPoints = new List<GameObject>();

    private void Start()
    {
        onTriggerFight += TriggerAttack;
        

        List<GameObject> gameObjects = new List<GameObject>();
        gameObject.GetChildGameObjects(gameObjects);

        // Get all hitpoints that are children of this object...
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<FencingHitPoint>() != null)
            {
                _hitPoints.Add(gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        onTriggerFight -= TriggerAttack;
    }

    private void TriggerAttack()
    {
        Attack(2);
    }

    private IEnumerator Attack(float pWaitTime)
    {
        yield return new WaitForSeconds(pWaitTime);

        
    }
}
