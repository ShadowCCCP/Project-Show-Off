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
        onTriggerFight += NextStage;
        

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
        onTriggerFight -= Attack;
    }



    private IEnumerator PlanAttack(float pWaitTime)
    {
        yield return new WaitForSeconds(pWaitTime);


    }

    private void Attack()
    {

    }
}
