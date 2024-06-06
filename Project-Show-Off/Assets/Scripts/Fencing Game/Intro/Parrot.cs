using System.Collections;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    private SoundPlayer _soundPlayer;

    [SerializeField] float timeBeforeWarning;
    [SerializeField] float pirateSpawnTimeAfterGrab;

    [SerializeField] Vector3 pirateSpawnPos;

    private bool _pirateWarningPlayed;

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
        StartCoroutine(InstantiatePirate());
    }

    private IEnumerator InstantiatePirate()
    {
        yield return new WaitForSeconds(pirateSpawnTimeAfterGrab);


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
