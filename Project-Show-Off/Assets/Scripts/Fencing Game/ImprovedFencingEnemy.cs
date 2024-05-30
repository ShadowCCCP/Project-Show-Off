using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImprovedFencingEnemy : MonoBehaviour
{
    public enum SideHit { Left, Right, Forward }

    private enum FencingState { Intro, Idle, Walk, Attack, Stunned }
    private FencingState _currentState = FencingState.Intro;

    [Tooltip("The walking distance. Counts for both for- and backwards.")]
    [SerializeField] float walkDistance = 2.0f;
    [Tooltip("Decides how long it takes for the pirate to cover the walking distance.")]
    [SerializeField] float walkTime = 2.0f;
    [Tooltip("Decided how many times the player/pirate needs to move in order to win.")]
    [SerializeField] int stageMaxCount = 2;

    [SerializeField] float idleWaitTime = 3.0f;

    private const int _maxAttackQueueCount = 3;
    private int _currentAttackCount;

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
    }

    public void Move(bool pForward = true)
    {
        _currentState = FencingState.Walk;

        if (pForward)
        {
            _anim.SetTrigger("WalkForward");
            // Move pirate forward...
            LeanTween.move(gameObject, transform.position + (transform.forward * walkDistance), walkTime).setOnComplete(() =>
            {
                if (_currentState != FencingState.Intro) { _currentFightStage++; }
                _anim.SetTrigger("StopWalk");
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
            });
        }

        TransitionState();
    }

    public void Attack()
    {
        // TODO:
        // Activate sword trigger collider...

        int randomAttack = Random.Range(0, 2);

        // If max amount of attacks isn't reached...
        if (_attacksDone.Count < _currentAttackCount)
        {
            // Get a random, but valid number and trigger animation...
            while (_attacksDone.Contains(randomAttack))
            {
                randomAttack = Random.Range(0, 2);
            }
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
        }
    }

    private void ProcessCurrentStage()
    {
        if (_currentFightStage >= stageMaxCount || _currentFightStage <= -stageMaxCount)
        {
            // Kick the player off the plank...
            if (_currentFightStage >= stageMaxCount)
            {

            }
            // Pirate falls into water...
            else if (_currentFightStage <= -stageMaxCount)
            {
                _anim.SetBool("WaterFall", true);
            }

            _gameCompleted = true;
        }
    }

    private IEnumerator InitiateAttack()
    {
        yield return new WaitForSeconds(idleWaitTime);
        Attack();
    }


    private void ResetValues()
    {
        _anim.SetInteger("Attack", -1);
        _anim.SetBool("GotBlocked", false);
        _anim.SetBool("WaterFall", false);
        _anim.SetBool("StunNotDamaged", false);
    }

    // Methods to be triggered by animator events...
    public void TransitionState()
    {
        if (_currentState == FencingState.Intro)
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
        _blockCount++;
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

            case SideHit.Forward:
                _anim.SetTrigger("HitForward");
                break;
        }

        _gotHit = true;
    }
}
