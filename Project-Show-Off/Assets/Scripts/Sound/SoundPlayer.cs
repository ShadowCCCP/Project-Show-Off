using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] _audioClips;

    [SerializeField] bool playOnAwake;
    [SerializeField] bool loop;
    [SerializeField] [Range(0, 1)] float spatialBlend;
    [SerializeField] [Range(0, 1)] float volume = 1;

    private AudioSource _audioSource;
    private int _currentClipIndex;

    private void Awake()
    {
        // Get audioSource if available / Create one otherwise...
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();

            if (ClipCount() > 0) { SetClip(0); }
            else { Debug.LogError(Useful.GetHierarchy(transform) + "\nSoundPlayer: No audioClips attached to the script."); }
        }

        _audioSource.loop = loop;
        _audioSource.spatialBlend = spatialBlend;
        _audioSource.volume = volume;
    }

    private void Start()
    {
        if (playOnAwake) { PlayRandom(); }
    }

    public void Play()
    {
        // Get the first audioClip of the array, if none is selected...
        if (_audioSource.clip == null && ClipCount() > 0) { SetClip(0); }
        PlaySound();
    }

    public void Stop()
    {
        _audioSource.Stop();
    }

    public void PlayNext()
    {
        // Get next sound in the array...
        int nextIndex = _currentClipIndex + 1;
        if (nextIndex >= ClipCount()) { nextIndex = 0; }

        SetClip(nextIndex);
        PlaySound();
    }

    public void PlayRandom()
    {
        GetRandomClip();
        PlaySound();
    }

    public void PlayRandom(int pIndexFrom, int pIndexTo)
    {
        GetRandomClip(pIndexFrom, pIndexTo);
        PlaySound();
    }

    public void PlaySpecific(int pSoundIndex)
    {
        if (pSoundIndex > 0 && pSoundIndex < _audioClips.Length)
        {
            SetClip(pSoundIndex);
        }

        PlaySound();
    }

    private void PlaySound()
    {
        if (_audioSource.clip != null) _audioSource.PlayDelayed(0);
        else
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nSoundPlayer: Not able to play sound. No audioClip attached to either script or existing audioSource.");
        }
    }

    private void GetRandomClip()
    {
        if (ClipCount() > 0)
        {
            int soundIndex = UnityEngine.Random.Range(0, _audioClips.Length);
            SetClip(soundIndex);
        }
        else
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nSoundPlayer: No audioClip attached to the script.");
        }
    }

    private void GetRandomClip(int pIndexFrom, int pIndexTo)
    {
        // Check for out of bounds errors...
        if (pIndexFrom > _audioClips.Length || pIndexFrom < 0 ||
            pIndexTo > _audioClips.Length || pIndexTo < 0 || pIndexTo < pIndexFrom)
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nSoundPlayer: GetRandomClip() got indexes out of bounds.");
            return;
        }

        if (ClipCount() > 0)
        {
            int soundIndex = UnityEngine.Random.Range(pIndexFrom, pIndexTo);
            SetClip(soundIndex);
        }
        else
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nSoundPlayer: No audioClip attached to the script.");
        }
    }

    private void SetClip(int pClipIndex)
    {
        _currentClipIndex = pClipIndex;
        _audioSource.clip = _audioClips[pClipIndex];
    }

    // For state checking...
    public bool IsPlaying()
    {
        return _audioSource.isPlaying;
    }

    public int ClipCount()
    {
        return _audioClips.Length;
    }
}
