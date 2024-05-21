using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] AudioClip[] _audioClip;

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

            _audioSource.loop = loop;
            _audioSource.spatialBlend = spatialBlend;
            GetRandomClip();
        }
    }

    private void Start()
    {
        if (!_audioSource.playOnAwake && playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        GetRandomClip();

        if (_audioSource.clip != null) _audioSource.PlayDelayed(0);
        else
        {
            Debug.LogError("AudioPlayer: No audioClip attached to either script or existing audioSource.");
        }
    }

    public void PlaySpecific(int pSoundIndex)
    {
        if (pSoundIndex > 0 && pSoundIndex < _audioClip.Length)
        {
            _audioSource.clip = _audioClip[pSoundIndex];
        }

        if (_audioSource.clip != null) _audioSource.PlayDelayed(0);
        else
        {
            Debug.LogError("AudioPlayer: No audioClip attached to either script or existing audioSource.");
        }
    }

    private void GetRandomClip()
    {
        int soundIndex = Random.Range(0, _audioClip.Length);
        _audioSource.clip = _audioClip[soundIndex];
    }
}
