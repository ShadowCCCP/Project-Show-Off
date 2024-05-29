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

    [SerializeField] int stageCount = 2;

    private Animator _anim;

    private int _currentFightStage;

    private List<int> _attacksDone = new List<int>();

    private bool _gotHit;

    private bool _gameCompleted;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void Move(bool pForward = true)
    {
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
    }

    public void Attack()
    {
        int randomAttack = Random.Range(0, 2);

        // If max amount of attacks isn't reached...
        if (_attacksDone.Count < 3)
        {
            // Get a random, but valid number...
            while (_attacksDone.Contains(randomAttack))
            {
                randomAttack = Random.Range(0, 2);
            }
            _anim.SetInteger("Attack", randomAttack);
        }
        else
        {
            _anim.SetTrigger("FinishedAttack");
        }
    }

    private void ProcessCurrentStage()
    {
        if (_currentFightStage >= stageCount || _currentFightStage <= -stageCount)
        {
            // Kick the player off the plank...
            if (_currentFightStage >= stageCount)
            {

            }
            // Pirate falls into water...
            else if (_currentFightStage <= -stageCount)
            {
                _anim.SetBool("WaterFall", true);
            }

            _gameCompleted = true;
        }
    }

    // Methods triggered outside this class...
    public void GotBlocked()
    {

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
