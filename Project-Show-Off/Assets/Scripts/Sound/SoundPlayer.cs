using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip _audioClip;

    [SerializeField] bool playOnAwake;
    [SerializeField] bool loop;
    [SerializeField] [Range(0, 1)] float spatialBlend;
    [SerializeField] [Range(0, 1)] float volume;

    private AudioSource _audioSource;

    private void Awake()
    {
        // Get audioSource if available / Create one otherwise...
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();

            _audioSource.playOnAwake = playOnAwake;
            _audioSource.loop = loop;
            _audioSource.spatialBlend = spatialBlend;

            if (_audioClip != null) { _audioSource.clip = _audioClip; }
            else 
            {
                Debug.LogError("AudioPlayer: No audioClip attached to script.");
            }
        }
    }

    public void Play()
    {
        if (_audioSource.clip != null) _audioSource.PlayDelayed(0);
        else
        {
            Debug.LogError("AudioPlayer: No audioClip attached to either script or existing audioSource.");
        }
    }
}
