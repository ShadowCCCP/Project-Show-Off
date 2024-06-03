using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedFencingEnemy : MonoBehaviour
{
    // TODO:
    // Add trigger collider detection to interact with the player's sword (events or object references?)
    // Third attack stage has four attacks?
    // Fix stagger phase

    public enum SideHit { Left, Right, Front }

    private enum FencingState { Intro, Idle, Walk, Taunt, Attack, Stunned }
    private FencingState _currentState = FencingState.Intro;

    public event Action onPlayerDefeated;

    [Tooltip("The walking distance. Counts for both for- and backwards.")]
    [SerializeField] float walkDistance = 2.0f;
    [Tooltip("Decides how long it takes for the pirate to cover the walking distance.")]
    [SerializeField] float walkTime = 2.0f;
    [Tooltip("Decided how many times the player/pirate needs to move in order to win.")]
    [SerializeField] int stageMaxCount = 3;

    [SerializeField] float idleTime = 3.0f;
    [SerializeField] float stunTime = 5.0f;

    private const int _maxAttackQueueCount = 3;
    private int _currentAttackCount = 1;

    private Animator _anim;

    // This is to check if either the player or the pirate has reached the end of the plank...
    private int _currentFightStage;

    private List<int> _attacksDone = new List<int>();
    private int _blockCount;

    private bool _gotHit;

    private bool _gameCompleted;

    private void Start()
    {
        _anim = GetComponent<Animator>();

        PlayerSword.onSwordHit += GotBlocked;
    }

    private void OnDestroy()
    {
        PlayerSword.onSwordHit -= GotBlocked;
    }

    private void Update()
    {
        Debug.Log(_currentState);

        if (Input.GetKeyDown(KeyCode.B))
        {
            _blockCount = _currentAttackCount;
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            SideGotHit(SideHit.Left);
        }
    }

    public void Move(bool pForward = true)
    {
        _currentState = FencingState.Walk;

        if (pForward)
        {
            Debug.Log("Walk Forward animation!");
            _anim.SetTrigger("WalkForward");
            // Move pirate forward...
            LeanTween.move(gameObject, transform.position + (transform.forward * walkDistance), walkTime).setOnComplete(() =>
            {
                if (_currentState != FencingState.Intro) { _currentFightStage++; }
                _anim.SetTrigger("StopWalk");
                ProcessCurrentStage();
                TransitionState();
            });
        }
        else
        {
            Debug.Log("Walk Backward animation!");
            _anim.SetTrigger("WalkBackward");
            // Move pirate backward...
            LeanTween.move(gameObject, transform.position - (transform.forward * walkDistance), walkTime).setOnComplete(() =>
            {
                _currentFightStage--;
                _anim.SetTrigger("StopWalk");
                TransitionState();
            });
        }
    }

    public void Attack()
    {
        // TODO:
        // Activate sword trigger collider...

        int randomAttack = UnityEngine.Random.Range(0, _maxAttackQueueCount);

        // If max amount of attacks isn't reached...
        if (_attacksDone.Count < _currentAttackCount)
        {
            // Get a random, but valid number and trigger animation...
            while (_attacksDone.Contains(randomAttack))
            {
                randomAttack = UnityEngine.Random.Range(0, _maxAttackQueueCount);
            }
            _attacksDone.Add(randomAttack);
            _anim.SetInteger("Attack", randomAttack);
        }
        else
        {
            CheckAttackOutcome();

            // Increase the amount of attacks done, if max not reached yet...
            if (_currentAttackCount < _maxAttackQueueCount)
            {
                _currentAttackCount++;
            }

            // Finish attack animation and clear the attackDone list...
            _anim.SetTrigger("FinishedAttack");
            _attacksDone.Clear();
        }
    }

    private void CheckAttackOutcome()
    {
        if (_blockCount >= _currentAttackCount)
        {
            _anim.SetBool("GotBlocked", true);

            StartCoroutine(EvaluateStagger());

            // TODO:
            // Activate side hit colliders...
            
        }
        else { _currentState = FencingState.Taunt; }
    }

    private void ProcessCurrentStage()
    {
        if (_currentFightStage >= stageMaxCount || _currentFightStage <= -stageMaxCount)
        {
            Debug.Log("Game completed");

            // Kick the player off the plank...
            if (_currentFightStage >= stageMaxCount)
            {
                // Invoke defeat player event...
                if (onPlayerDefeated != null) { onPlayerDefeated(); }
            }
            // Pirate falls into water...
            else if (_currentFightStage <= -stageMaxCount)
            {
                // Make pirate fall off the plank and destroy gameObject after some time...
                _anim.SetBool("WaterFall", true);
                StartCoroutine(DestroyAfterDelay());
            }

            _gameCompleted = true;
        }
    }

    private IEnumerator InitiateAttack()
    {
        // Idle before attack...
        ResetValues();
        _currentState = FencingState.Idle;
        yield return new WaitForSeconds(idleTime);

        // Attack afterwards...
        _currentState = FencingState.Attack;
        Attack();
    }

    private IEnumerator EvaluateStagger()
    {
        _currentState = FencingState.Stunned;
        yield return new WaitForSeconds(stunTime);

        if (_gotHit)
        {
            ProcessCurrentStage();

            // Move backwards if game not finished...
            if (!_gameCompleted) { Move(false); }
        }
        else
        {
            _anim.SetTrigger("StunNotDamaged");
        }
    }


    private void ResetValues()
    {
        _anim.SetInteger("Attack", -1);
        _anim.SetBool("GotBlocked", false);
        _anim.SetBool("WaterFall", false);
        _anim.SetBool("StunNotDamaged", false);
        _blockCount = 0;
        _gotHit = false;
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    // Methods to be triggered by animator events...
    public void TransitionState()
    {
        // Don't transition to anything if game is complete...
        if (_gameCompleted) { return; }

        if (_currentState == FencingState.Intro || _currentState == FencingState.Taunt)
        {
            _anim.SetTrigger("WalkForward");
            _currentState = FencingState.Walk;
            Move();
        }
        else if (_currentState == FencingState.Walk)
        {
            StartCoroutine(InitiateAttack());
        }
    }

    // Methods triggered outside this class...
    public void GotBlocked()
    {
        if (_currentState == FencingState.Attack) { _blockCount++; }
    }

    public void SideGotHit(SideHit pSide)
    {
        switch (pSide)
        {
            case SideHit.Left:
                _anim.SetTrigger("HitLeft");
                break;

            case SideHit.Right:
                _anim.SetTrigger("HitRight");
                break;

            case SideHit.Front:
                _anim.SetTrigger("HitFront");
                break;
        }

        if (!_gotHit) _gotHit = true;
    }
}
