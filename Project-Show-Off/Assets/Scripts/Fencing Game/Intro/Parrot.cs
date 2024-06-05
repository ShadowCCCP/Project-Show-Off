using System.Collections;
using UnityEngine;

public class Parrot : MonoBehaviour
{
    private SoundPlayer _soundPlayer;

    [SerializeField] float timeBeforeWarning;
    private bool _pirateWarningPlayed;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();

        // Play intro sound...

    }

    private void Update()
    {
        CheckForPirates();
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
