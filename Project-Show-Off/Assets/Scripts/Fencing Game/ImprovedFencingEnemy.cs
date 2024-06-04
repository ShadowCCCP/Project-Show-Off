using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedFencingEnemy : MonoBehaviour
{
    private bool _debugMode = false;

    public enum SideHit { Left, Right, Front }

    private enum FencingState { Intro, Idle, Walk, Taunt, Attack, Stunned }
    private FencingState _lastState;
    private FencingState _currentState;
    private FencingState CurrentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value) 
            {
                _lastState = _currentState;
                _currentState = value;
            }
        }
    }

    public event Action onPlayerDefeated;
    public event Action onToggleBodyCols;
    public event Action onToggleSwordCol;

    [Tooltip("The walking distance. Counts for both for- and backwards.")]
    [SerializeField] float walkDistance = 2.0f;
    [Tooltip("Decides how long it takes for the pirate to cover the walking distance.")]
    [SerializeField] float walkTime = 2.0f;
    [Tooltip("Decided how many times the player/pirate needs to move in order to win.")]
    [SerializeField] int stageMaxCount = 3;

    [Space]

    [SerializeField] float idleTime = 3.0f;
    [SerializeField] float stunTime = 5.0f;

    [Space]

    [Tooltip("Speed multiplier for the backward attack animation, when player blocks his attack.")] 
    [SerializeField] float counteredSpeedMultiplier = 1.2f;

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
        if (_debugMode) { EasierTesting(); }
    }

    private void EasierTesting()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GotBlocked();
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            SideGotHit(SideHit.Left);
        }
    }

    public void Move(bool pForward = true)
    {
        CurrentState = FencingState.Walk;

        if (pForward)
        {
            _anim.SetTrigger("WalkForward");
            // Move pirate forward...
            LeanTween.move(gameObject, transform.position + (transform.forward * walkDistance), walkTime).setOnComplete(() =>
            {
                if (_lastState != FencingState.Intro) { _currentFightStage++; }
                _anim.SetTrigger("StopWalk");
                ProcessCurrentStage();
                TransitionState();
            });
        }
        else
        {
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
        // Activate sword collider...
        TriggerColEvent(false);

        // Reset the animation speed for the attack...
        _anim.SetFloat("AttackSpeed", 1);

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
            _anim.SetFloat("Attacks", randomAttack);
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
        // Deactivate sword collider...
        TriggerColEvent(false);

        if (_blockCount >= _currentAttackCount)
        {
            CurrentState = FencingState.Stunned;
            _anim.SetBool("GotBlocked", true);

            StartCoroutine(EvaluateStagger());

            // Activate body trigger colliders...
            TriggerColEvent();
        }
        else { CurrentState = FencingState.Taunt; }
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

            ResetValues();
            _gameCompleted = true;
        }
    }

    private IEnumerator InitiateAttack()
    {
        // Idle before attack...
        ResetValues();
        CurrentState = FencingState.Idle;
        yield return new WaitForSeconds(idleTime);

        // Attack afterwards...
        ResetValues();
        CurrentState = FencingState.Attack;
        Attack();
    }

    private IEnumerator EvaluateStagger()
    {
        yield return new WaitForSeconds(stunTime);

        // Deactivate body trigger colliders...
        TriggerColEvent();

        if (_gotHit)
        {
            ProcessCurrentStage();

            // Move backwards if game not finished...
            if (!_gameCompleted) { Move(false); }
        }
        else
        {
            _anim.SetTrigger("StunNotDamaged");
            StartCoroutine(InitiateAttack());
        }
    }


    private void ResetValues()
    {
        _anim.SetFloat("Attacks", -1);
        _anim.SetFloat("Hits", -1);
        _anim.SetBool("GotBlocked", false);
        _blockCount = 0;
        _gotHit = false;
    }

    private void TriggerColEvent(bool pBody = true)
    {
        // Trigger one of the toggle events for the pirate colliders...
        if (pBody)
        {
            if (onToggleBodyCols != null) { onToggleBodyCols(); }
        }
        else
        {
            if (onToggleSwordCol != null) { onToggleSwordCol(); }
        }
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

        if (CurrentState == FencingState.Intro || CurrentState == FencingState.Taunt)
        {
            _anim.SetTrigger("WalkForward");
            CurrentState = FencingState.Walk;
            Move();
        }
        else if (CurrentState == FencingState.Walk)
        {
            StartCoroutine(InitiateAttack());
        }
        else if (CurrentState == FencingState.Stunned)
        {
            _anim.SetFloat("Hits", 3);
        }
    }

    // Methods triggered outside this class...
    public void GotBlocked()
    {
        if (CurrentState == FencingState.Attack) 
        { 
            _blockCount++;

            // Reverse the attack animation...
            _anim.SetFloat("AttackSpeed", -counteredSpeedMultiplier);
        }
    }

    public void SideGotHit(SideHit pSide)
    {
        switch (pSide)
        {
            case SideHit.Left:
                _anim.SetFloat("Hits", 0);
                break;

            case SideHit.Right:
                _anim.SetFloat("Hits", 1);
                break;

            case SideHit.Front:
                _anim.SetFloat("Hits", 2);
                break;
        }

        if (!_gotHit) _gotHit = true;
    }
}
