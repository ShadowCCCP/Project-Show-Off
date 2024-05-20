using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

// TODO:
// Remove sword collision logic and replace it with shield spots to hold sword onto for blocking...

public class FencingEnemy : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] GameObject hitPointsHolder;
    [SerializeField] GameObject defensePointsHolder;

    [SerializeField] float timeToBlock = 8.0f;
    [SerializeField] float staggerDuration = 6.0f;

    [SerializeField] int showHitPointAmount = 3;
    private int _hitPointsDone = 0;

    private float _staggerCooldown = 0.0f;
    private bool _isStaggering;
    private bool _triggeredStagger;

    private List<GameObject> _hitPoints = new List<GameObject>();
    private List<GameObject> _finishedHitPoints = new List<GameObject>();
    private int _currentHitPoint;

    private List<GameObject> _defensePoints = new List<GameObject>();
    private List<GameObject> _finishedDefensePoints = new List<GameObject>();
    private int _currentDefensePoint;

    private bool _initialized;

    private Animator _anim;

    private bool _gotBlocked;

    private void Start()
    {
        if (hitPointsHolder == null)
        {
            Debug.LogError("FencingEnemy: HitPointHolder reference missing...");
        }

        if (defensePointsHolder == null)
        {
            Debug.LogError("FencingEnemy: DefensePointHolder reference missing...");
        }

        _anim = GetComponent<Animator>();

        // Get all hitpoint objects...
        List<GameObject> hitPointHolderObjects = new List<GameObject>();
        hitPointsHolder.GetChildGameObjects(hitPointHolderObjects);

        // Get all hitpoints that are children of this object and hide them...
        foreach (GameObject gameObject in hitPointHolderObjects)
        {
            if (gameObject.GetComponent<FencingHitPoint>() != null)
            {
                _hitPoints.Add(gameObject);
                gameObject.SetActive(false);
            }
        }

        // Get all defensepoint objects...
        List<GameObject> defensePointHolderObjects = new List<GameObject>();
        defensePointsHolder.GetChildGameObjects(defensePointHolderObjects);

        // Get all hitpoints that are children of this object and hide them...
        foreach (GameObject gameObject in defensePointHolderObjects)
        {
            if (gameObject.GetComponent<FencingDefensePoint>() != null)
            {
                _defensePoints.Add(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        UpdateStagger();
    }

    private void UpdateStagger()
    {
        if (_isStaggering)
        {
            // Do this only once...
            if (!_triggeredStagger)
            {
                // Trigger stagger animation and clear finishedHitPoints list...
                _finishedHitPoints.Clear();
                _anim.SetBool("Staggering", true);
                _triggeredStagger = true;
            }

            if (_hitPointsDone < showHitPointAmount && _staggerCooldown < staggerDuration)
            {
                // Increase timer...
                _staggerCooldown += Time.deltaTime;
                ShowHitPoint();
            }
            else
            {
                if (_hitPointsDone >= showHitPointAmount) { health--; }

                // Hide hitpoint and stop stagger animation...
                HideHitPoint();
                _anim.SetTrigger("StopStagger");

                // Reset stagger values...
                _staggerCooldown = 0;
                _isStaggering = false;
            }
        }
    }

    private void CheckAttackOutcome()
    {
        if (_gotBlocked) { TriggerStagger(); }
        _gotBlocked = false;
    }

    private void ManageState()
    {
        // If health is still above 0, proceed to attack... 
        if (health > 0)
        {
            if (!_initialized)
            {
                // Do the intro animation...
                hitPointsHolder.SetActive(true);
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
        _finishedHitPoints.Clear();
        _hitPointsDone = 0;

        // Start stagger animation...
        _anim.SetBool("Staggering", true);
        _isStaggering = true;
    }

    private void ShowHitPoint()
    {
        // Pick random, but valid hitpoint to be visible...
        if (!_hitPoints[_currentHitPoint].activeSelf)
        {
            bool validHitPoint = false;

            while (!validHitPoint)
            {
                // If all hitpoints were hit but enemy is still alive, just reuse finished hitpoints again...
                if (_finishedHitPoints.Count == _hitPoints.Count) { validHitPoint = true; }

                _currentHitPoint = UnityEngine.Random.Range(0, _hitPoints.Count);

                if (!validHitPoint && !_finishedHitPoints.Contains(_hitPoints[_currentHitPoint]))
                {
                    validHitPoint = true;
                }
            }

            // Make selected hitpoint visible
            _hitPoints[_currentHitPoint].SetActive(true);
        }
    }

    private void HideHitPoint()
    {
        if (_hitPoints[_currentHitPoint] != null)
        {
            _hitPoints[_currentHitPoint].SetActive(false);
        }
    }

    private void ShowDefensePoint()
    {
        // Pick random, but valid hitpoint to be visible...
        if (!_defensePoints[_currentDefensePoint].activeSelf)
        {
            bool validHitPoint = false;

            while (!validHitPoint)
            {
                // If all hitpoints were hit but enemy is still alive, just reuse finished hitpoints again...
                if (_finishedDefensePoints.Count == _defensePoints.Count) { validHitPoint = true; }

                _currentDefensePoint = UnityEngine.Random.Range(0, _defensePoints.Count);

                if (!validHitPoint && !_finishedDefensePoints.Contains(_defensePoints[_currentDefensePoint]))
                {
                    validHitPoint = true;
                }
            }

            // Make selected hitpoint visible
            _defensePoints[_currentDefensePoint].SetActive(true);
        }
    }

    private void HideDefensePoint()
    {
        if (_defensePoints[_currentDefensePoint] != null)
        {
            _defensePoints[_currentDefensePoint].SetActive(false);
            _defensePoints[_currentDefensePoint].GetComponent<FencingDefensePoint>().ResetState();
        }
    }

    private void TriggerAttack()
    {
        // Reset animator values...
        _anim.SetBool("Staggering", false);

        StartCoroutine(Attack(timeToBlock));
        ShowDefensePoint();
    }

    private IEnumerator Attack(float pWaitTime)
    {
        yield return new WaitForSeconds(pWaitTime);

        // Check if sword is inside area...
        if (_defensePoints[_currentDefensePoint].GetComponent<FencingDefensePoint>().GetState()) 
        { 
            _gotBlocked = true; 
        }
        HideDefensePoint();

        // Do the attack animation...
        _anim.SetTrigger("Attack");
    }

    // Methods triggered outside this class...
    public void HitPointHit(GameObject pGameObject)
    {
        _hitPointsDone++;
        _finishedHitPoints.Add(pGameObject);
        HideHitPoint();
    }

    public void StartIntro()
    {
        // This should be false anyways, but just in case...
        _initialized = false;
        ManageState();
    }
}