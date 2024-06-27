using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the fight sequence of the pirate during the fencing minigame.
/// It's heavily tied to the actual animator with its events and parameters.
/// It will start executing on Start(), not waiting for any other queues.
/// </summary>

public class ImprovedFencingEnemy : MonoBehaviour
{
    private bool _debugMode = true;

    public enum SideHit { Left, Right, Front }

    private enum FencingState { Intro, Idle, Walk, Taunt, Attack, Stunned, Dazed, UnhitDazed }
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

    public event Action onToggleBodyCols;
    public event Action onToggleSwordCol;

    [Tooltip("The walking distance. Counts for both for- and backwards.")]
    [SerializeField] float walkDistance = 2.0f;
    [Tooltip("Decides how long it takes for the pirate to cover the walking distance.")]
    [SerializeField] float walkTime = 2.0f;
    [Tooltip("Decides how many times the player/pirate needs to move in order to win.")]
    [SerializeField] int stageMaxCount = 3;
    [Tooltip("Decides how many times the player can win or lose before the decisive round")]
    [SerializeField] int decisiveTurn = 6;

    [Space]

    [SerializeField] float idleTime = 3.0f;
    [SerializeField] float stunTime = 5.0f;

    [Space]

    [Tooltip("Speed multiplier for the backward attack animation, when player blocks his attack.")] 
    [SerializeField] float counteredSpeedMultiplier = 1.2f;

    private int _decisiveCount;

    private bool _specialAttack;

    private const int _maxAttackQueueCount = 3;
    private int _currentAttackCount = 1;

    private Animator _anim;

    // This is to fix any unwanted movement which makes the pirate look like he's floating outside the plank...
    private float _initialX;
    private float _initialZ;

    // This is to check if either the player or the pirate has reached the end of the plank...
    private int _currentFightStage;

    private List<int> _attacksDone = new List<int>();
    private int _blockCount;

    private bool _gotHit;
    private bool _gotBlocked;

    // Hit animation cooldown...
    private float _hitWaitTime = 1.5f;
    private float _hitCurrentTime;

    private bool _gameCompleted;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _initialX = transform.position.x;
        _initialZ = transform.position.z;

        PlayerSword.onSwordHit += GotBlocked;
    }

    private void OnDestroy()
    {
        PlayerSword.onSwordHit -= GotBlocked;
    }

    private void Update()
    {
        if (_debugMode) { EasierTesting(); }

        FixPosition();
        UpdateTimer();
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

    private void FixPosition()
    {
        // This is definetely not the best fix, but it does the job for now...
        float rotationY = transform.rotation.eulerAngles.y;
        if (rotationY % 180 == 0)
        {
            if (transform.position.x != _initialX)
            {
                transform.position = new Vector3(_initialX, transform.position.y, transform.position.z);
            }
        }
        else
        {
            if (transform.position.z != _initialZ)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, _initialZ);
            }
        }
    }

    private void UpdateTimer()
    {
        _hitCurrentTime += Time.deltaTime;
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
            // Move pirate backward...
            // No need to trigger the animation value for this, as it's only after the daze...
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
        // Reset the animation speed for the attack...
        _anim.SetFloat("AttackSpeed", 1);

        int randomAttack = UnityEngine.Random.Range(0, 3);

        // If max amount of attacks isn't reached and attack got blocked...
        if (_attacksDone.Count < _currentAttackCount && (_gotBlocked || _attacksDone.Count == 0))
        {
            // Reset block value to see if this attack got blocked...
            _gotBlocked = false;

            // Activate sword collider only at the very first attack...
            if (_attacksDone.Count == 0) { TriggerColEvent(false); }

            // Check if this might be the pirates final attack...
            if (_decisiveCount >= decisiveTurn - 1 || _currentFightStage >= stageMaxCount)
            {
                _specialAttack = true;

                // Make the random attack not so random...
                randomAttack = 3;

                // And add more into the list to only attack once...
                while (_attacksDone.Count < _currentAttackCount)
                {
                    int filler = UnityEngine.Random.Range(0, 2);
                    _attacksDone.Add(filler);
                }
            }
            else
            {
                // Get a random, but valid number and trigger animation...
                while (_attacksDone.Contains(randomAttack))
                {
                    randomAttack = UnityEngine.Random.Range(0, 3);
                }
                _attacksDone.Add(randomAttack);
            }

            _anim.SetInteger("Attacks", randomAttack);
        }
        else
        {
            // Increase total count of rounds done...
            _decisiveCount++;

            // Deactivate sword collider...
            TriggerColEvent(false);

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
            CurrentState = FencingState.Stunned;
            _anim.SetBool("GotBlocked", true);

            StartCoroutine(EvaluateStagger());

            // Activate body trigger colliders...
            TriggerColEvent();
        }
        else 
        {
            int randomTaunt = UnityEngine.Random.Range(0, 3);
            _anim.SetInteger("Taunts", randomTaunt);

            EventBus<OnPlayerHitEvent>.Publish(new OnPlayerHitEvent());

            CurrentState = FencingState.Taunt; 
        }

        // Reset special attack...
        if (_specialAttack) { _specialAttack = false; }
    }

    private void ProcessCurrentStage()
    {
        // Check if game is completed...
        if (_currentFightStage >= stageMaxCount || _currentFightStage <= -stageMaxCount || _decisiveCount >= decisiveTurn)
        {
            Debug.Log("Game completed");

            // Kick the player off the plank...
            if (_currentFightStage >= stageMaxCount || (_decisiveCount >= decisiveTurn && !_gotHit))
            {
                // Invoke defeat player event...
                EventBus<LevelFinishedEvent>.Publish(new LevelFinishedEvent(5));
            }
            // Pirate falls into water...
            else if (_currentFightStage <= -stageMaxCount || (_decisiveCount >= decisiveTurn && _gotHit))
            {
                EventBus<OnPirateDefeatedEvent>.Publish(new OnPirateDefeatedEvent());

                // Make pirate fall off the plank and destroy gameObject after some time...
                _anim.SetBool("WaterFall", true);
                StartCoroutine(DestroyAfterDelay());
            }

            ResetValues();
            _gameCompleted = true;
        }
    }

    private void TriggerGameFinish(float pDelay)
    {
        EventBus<LevelFinishedEvent>.Publish(new LevelFinishedEvent(pDelay));
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
            if (!_gameCompleted) 
            {
                CurrentState = FencingState.Dazed;
                _anim.SetTrigger("WalkBackward");
            }
        }
        else
        {
            CurrentState = FencingState.UnhitDazed;
            _anim.SetTrigger("StunNotDamaged");
        }
    }


    private void ResetValues()
    {
        _anim.SetInteger("Attacks", -1);
        _anim.SetInteger("Hits", -1);
        _anim.SetInteger("Taunts", -1);
        _anim.SetBool("GotBlocked", false);
        _blockCount = 0;
        _gotHit = false;
        _gotBlocked = false;
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
        yield return new WaitForSeconds(8);
        EventBus<LevelFinishedEvent>.Publish(new LevelFinishedEvent(0));
        Destroy(gameObject);
    }

    // Methods to be triggered by animator events...
    public void TransitionState()
    {
        // Don't transition to anything if game is complete...
        if (_gameCompleted) { return; }

        // To transition back to normal...
        if (CurrentState == FencingState.UnhitDazed || CurrentState == FencingState.Dazed)
        {
            _anim.SetTrigger("FinishDaze");
        }

        if (CurrentState == FencingState.Intro || CurrentState == FencingState.Taunt)
        {
            Move();
        }
        else if (CurrentState == FencingState.Walk || CurrentState == FencingState.UnhitDazed)
        {
            StartCoroutine(InitiateAttack());
        }
        else if (CurrentState == FencingState.Dazed)
        {
            Move(false);
        }
        else if (CurrentState == FencingState.Stunned)
        {
            _anim.SetInteger("Hits", 3);
        }
    }

    // When the enemy gets blocked, ensure it transitions further...
    public void CheckSwordSetBack()
    {
        // Only invoke Attack() if the attack got blocked...
        if (_gotBlocked)
        {
            // Weird bug fix...
            bool preventFiller = false;
            if (_attacksDone.Count == 0) 
            {
                // Fixes sword toggle...
                _attacksDone.Add(1);

                preventFiller = true; 
            }

            Attack();

            if (preventFiller) { _attacksDone.Clear(); }
        }
    }

    // The following methods are used to make the falling animation look appropriate...
    public void RotatePirate()
    {
        // Make him rotate either right or left randomly...
        int randomDir = UnityEngine.Random.Range(0, 2);
        if (randomDir == 0) { randomDir = 90; }
        else { randomDir = -90; }

        LeanTween.rotateY(gameObject, transform.eulerAngles.y + randomDir, 2.0f);
    }

    public void MoveForward()
    {
        LeanTween.move(gameObject, transform.position + (transform.forward * 5.5f) + (-transform.up * 5.5f), 2.0f);
    }

    // Methods triggered outside this class...
    public void GotBlocked()
    {
        if (CurrentState == FencingState.Attack && !_gotBlocked) 
        { 
            // To account for the single special attack...
            if (_specialAttack) { _blockCount = _currentAttackCount; }
            else { _blockCount++; }
            _gotBlocked = true;

            // Reverse the attack animation...
            _anim.SetFloat("AttackSpeed", -counteredSpeedMultiplier);
        }
    }

    public void SideGotHit(SideHit pSide)
    {
        // If hit animation timer is up...
        if (_hitCurrentTime >= _hitWaitTime)
        {
            switch (pSide)
            {
                case SideHit.Left:
                    _anim.SetInteger("Hits", 0);
                    break;

                case SideHit.Right:
                    _anim.SetInteger("Hits", 1);
                    break;

                case SideHit.Front:
                    _anim.SetInteger("Hits", 2);
                    break;
            }

            if (!_gotHit) { _gotHit = true; }

            // Reset timer...
            _hitCurrentTime = 0;
        }
    }
}