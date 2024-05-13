using System;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;

public class FencingEnemy : MonoBehaviour
{
    [SerializeField] int attackCount;
    private int _currentStage;

    private List<GameObject> _hitPoints = new List<GameObject>();

    private event Action _onStartSequence;
    private bool _isFighting;

    private void Start()
    {
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

    private void Update()
    {
        
    }

    private IEnumerator NextStage(float pWaitTime)
    {
        yield return new WaitForSeconds(5);


    }
}
