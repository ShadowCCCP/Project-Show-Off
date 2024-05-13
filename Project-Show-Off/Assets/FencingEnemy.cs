using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils;
using UnityEngine;

public class FencingEnemy : MonoBehaviour
{
    // This event is triggered as soon as a certain animation is finished
    // or the player enters the plank for the first time ...
    public event Action onTriggerSequence;
    public event Action onStartStagger;

    [SerializeField] int attackCount;
    private int _currentAttackCount;

    private List<GameObject> _hitPoints = new List<GameObject>();
    private int _currentHitPoint;

    private bool _initialized;

    private void Start()
    {
        onTriggerSequence += ManageState;
        onStartStagger += TriggerStagger;

        List<GameObject> gameObjects = new List<GameObject>();
        gameObject.GetChildGameObjects(gameObjects);

        // Get all hitpoints that are children of this object and hide them...
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<FencingHitPoint>() != null)
            {
                _hitPoints.Add(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDestroy()
    {
        onTriggerSequence -= TriggerAttack;
        onStartStagger -= TriggerStagger;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            TriggerStagger();
        }
    }

    private void ManageState()
    {
        if (!_initialized)
        {
            // Do the intro animation...
            
            _initialized = true;
            return;
        }

        TriggerAttack();

        // Increment attackCount and check if game is finished...
        _currentAttackCount++;
        if (_currentAttackCount >= attackCount)
        {
            // Minigame finished...
            // !! DO SOMETHING !!
        }
    }

    private void TriggerStagger()
    {
        int staggerDuration = 5;
        StartCoroutine(Stagger(staggerDuration));
    }

    private IEnumerator Stagger(float pWaitTime)
    {
        ShowHitPoint();
        // Do the stagger animation...
        

        // Loop the animation for this duration...
        yield return new WaitForSeconds(pWaitTime);

        HideHitPoint();
        // Transition to next attack...
        ManageState();
    }

    private void ShowHitPoint()
    {
        // Pick random hitpoint to be visible...
        _currentHitPoint = UnityEngine.Random.Range(0, _hitPoints.Count);
        _hitPoints[_currentHitPoint].SetActive(true);
    }

    private void HideHitPoint()
    {
        // Disable currently active hitpoint, if it still exists...
        if (_hitPoints[_currentHitPoint] != null)
        {
            _hitPoints[_currentHitPoint].SetActive(false);
            _hitPoints.Remove(_hitPoints[_currentHitPoint]);
        }
    }

    private void TriggerAttack()
    {
        int timeBeforeAttack = 2;
        StartCoroutine(Attack(timeBeforeAttack));
    }

    private IEnumerator Attack(float pWaitTime)
    {
        yield return new WaitForSeconds(pWaitTime);

        // Do the attack animation...

    }
}
