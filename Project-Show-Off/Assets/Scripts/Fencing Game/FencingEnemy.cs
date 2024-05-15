using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class FencingEnemy : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] GameObject hitPointsHolder;

    private List<GameObject> _hitPoints = new List<GameObject>();
    private List<GameObject> _finishedHitPoints = new List<GameObject>();
    private int _currentHitPoint;

    private bool _initialized;

    private Animator _anim;

    private bool _gotHit;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        List<GameObject> gameObjects = new List<GameObject>();
        hitPointsHolder.GetChildGameObjects(gameObjects);

        // Get all hitpoints that are children of this object and hide them...
        foreach (GameObject gameObject in gameObjects)
        {
            if (gameObject.GetComponent<FencingHitPoint>() != null)
            {
                _hitPoints.Add(gameObject);
                gameObject.SetActive(false);
            }
        }

        ManageState();
    }

    private void CheckAttackOutcome()
    {
        if (!_gotHit) { TriggerStagger(); }
        _gotHit = false;
    }

    private void ManageState()
    {
        // If health is still above 0, proceed to attack...
        if (health > 0)
        {
            if (!_initialized)
            {
                // Do the intro animation...
                _anim.SetTrigger("Intro");

                _initialized = true;
                return;
            }

            TriggerAttack();
        }
        else
        {
            // Minigame finished...
            // ! DO SOMETHING !
            Destroy(gameObject);
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
        _anim.SetBool("Staggering", true);

        // Loop the animation for this duration...
        yield return new WaitForSeconds(pWaitTime);

        HideHitPoint();
        // Stop Staggering and transition to next attack...
        _anim.SetTrigger("StopStagger");
    }

    private void ShowHitPoint()
    {
        bool validHitPoint = false;

        // Pick random, but valid hitpoint to be visible...
        while(!validHitPoint)
        {
            // If all hitPoints were hit but enemy is still alive, just reuse finished hitPoints again...
            if (_finishedHitPoints.Count == _hitPoints.Count) { validHitPoint = true; }

            _currentHitPoint = UnityEngine.Random.Range(0, _hitPoints.Count);

            if (!validHitPoint && !_finishedHitPoints.Contains(_hitPoints[_currentHitPoint]))
            {
                validHitPoint = true;
            }
        }

        _hitPoints[_currentHitPoint].SetActive(true);
    }

    private void HideHitPoint()
    {
        if (_hitPoints[_currentHitPoint] != null)
        {
            _hitPoints[_currentHitPoint].SetActive(false);
        }
    }

    private void TriggerAttack()
    {
        int timeBeforeAttack = 3;
        StartCoroutine(Attack(timeBeforeAttack));
    }

    private IEnumerator Attack(float pWaitTime)
    {
        yield return new WaitForSeconds(pWaitTime);

        // Do the attack animation...
        _anim.SetTrigger("Attack");
    }

    // Methods triggered outside this class...
    public void HitPointHit(GameObject pGameObject)
    {
        health--;
        _finishedHitPoints.Add(pGameObject);
        HideHitPoint();
    }

    public void SwordHit()
    {
        _gotHit = true;
        _anim.SetBool("Staggering", false);
    }

    public void SwordBlocked()
    {
        _anim.SetTrigger("SetBackAttack");
    }
}