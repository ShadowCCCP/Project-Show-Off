using System.Collections;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    private SoundPlayer _soundPlayer;

    [SerializeField] float timeBeforeWarning;
    [SerializeField] float pirateSpawnTimeAfterGrab;

    [SerializeField] GameObject piratePrefab;
    [SerializeField] Vector3 pirateSpawnPos;
    [SerializeField] Quaternion pirateRotation;

    private bool _pirateWarningPlayed;
    private bool _pirateSpawned;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();

        // Play intro sound...


        PlayerSword.onSwordGrabbed += QueueInPirateSpawn;
    }

    private void OnDestroy()
    {
        PlayerSword.onSwordGrabbed -= QueueInPirateSpawn;
    }

    private void Update()
    {
        //CheckForPirates();
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

    private void CheckForPirates()
    {
        if (!_soundPlayer.IsPlaying() && !_pirateWarningPlayed)
        {
            StartCoroutine(WarnAboutPirate());
            _pirateWarningPlayed = true;
        }
    }

    private IEnumerator WarnAboutPirate()
    {
        yield return new WaitForSeconds(timeBeforeWarning);

        _soundPlayer.PlayNext();
    }
}
