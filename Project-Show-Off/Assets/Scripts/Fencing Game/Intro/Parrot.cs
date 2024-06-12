using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class Parrot : MonoBehaviour
{
    private SoundPlayer _soundPlayer;

    private float _beginningWaitTime = 5.0f;
    private bool _playedIntro;

    [SerializeField] float timeBeforeWarning;
    [SerializeField] float pirateSpawnTimeAfterGrab;

    [Space]

    [SerializeField] List<Transform> movePositions;
    [SerializeField] float moveTimePerPoint = 2.0f;
    [SerializeField] float rotationTimePerPoint = 1.0f;

    [Space]

    [SerializeField] GameObject piratePrefab;
    [SerializeField] Vector3 pirateSpawnPos;
    [SerializeField] Quaternion pirateRotation;

    private int _currentMoveDestination;
    private bool _moving;

    private bool _pirateWarningPlayed;
    private bool _pirateSpawned;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();

        // Play intro sound...
        StartCoroutine(PlayIntro());

        PlayerSword.onSwordGrabbed += QueueInPirateSpawn;
    }

    private void OnDestroy()
    {
        PlayerSword.onSwordGrabbed -= QueueInPirateSpawn;
    }

    private void Update()
    {
        MovePosition();
        CheckForPirates();
    }

    private void MovePosition()
    {
        if (_currentMoveDestination < movePositions.Count && !_moving)
        {
            _moving = true;

            // Move parrot to the next movePosition...
            LeanTween.move(gameObject, movePositions[_currentMoveDestination].position, moveTimePerPoint).setOnComplete(() =>
            {
                _moving = false;
                _currentMoveDestination++;
            });

            // And Rotate it towards the destination...
            Vector3 direction = (movePositions[_currentMoveDestination].position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            LeanTween.rotate(gameObject, lookRotation.eulerAngles, rotationTimePerPoint);
        }
    }

    private void CheckForPirates()
    {
        if (!_soundPlayer.IsPlaying() && !_pirateWarningPlayed && _playedIntro)
        {
            StartCoroutine(WarnAboutPirate());
            _pirateWarningPlayed = true;
        }
    }

    private void QueueInPirateSpawn()
    {
        if (!_pirateSpawned)
        {
            StartCoroutine(InstantiatePirate());
        }
    }

    private IEnumerator InstantiatePirate()
    {
        _pirateSpawned = true;

        yield return new WaitForSeconds(pirateSpawnTimeAfterGrab);

        Instantiate(piratePrefab, pirateSpawnPos, pirateRotation, transform.parent);
    }

    private IEnumerator PlayIntro()
    {
        yield return new WaitForSeconds(_beginningWaitTime);

        _soundPlayer.Play();
        _playedIntro = true;
    }

    private IEnumerator WarnAboutPirate()
    {
        yield return new WaitForSeconds(timeBeforeWarning);

        _soundPlayer.PlayNext();
    }
}
