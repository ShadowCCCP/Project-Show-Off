using UnityEngine;

public class Parrot : MonoBehaviour
{
    private SoundPlayer _soundPlayer;

    private bool _pirateWarningPlayed;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();

        // Play intro sound...

    }

    private void Update()
    {
        
    }

    private void BewareOfPiratesCheck()
    {
        if (!_soundPlayer.IsPlaying() && !_pirateWarningPlayed)
        {

        }
    }
}
