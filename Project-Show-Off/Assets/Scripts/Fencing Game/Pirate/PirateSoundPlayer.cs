using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSoundPlayer : MonoBehaviour
{
    /// <summary>
    /// Plays all the sounds relevant to the pirate.
    /// All of them are triggered through the animator.
    /// 
    /// Indexes of all the sounds to be played:
    /// [ 0 - 5  ]  |   WoodSteps
    /// [ 6 - 8  ]  |   Stabbing
    /// [ 9 - 10 ]  |   Swoosh (Sword swing)
    /// [ 11 ]      |   Water Splash
    /// </summary>

    private SoundPlayer _soundPlayer;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();

        if (_soundPlayer == null)
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nPirateSoundPlayer: No SoundPlayer component attached.");
        }
    }

    public void PlayStep()
    {
        _soundPlayer.PlayRandom(0, 5);
    }

    public void PlayStab()
    {
        _soundPlayer.PlayRandom(6, 8);
    }

    public void PlaySwoosh()
    {
        _soundPlayer.PlayRandom(9, 10);
    }

    public void PlayWaterSplash()
    {
        _soundPlayer.PlaySpecific(11);
    }
}
