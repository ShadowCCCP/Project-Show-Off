using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Transform transitionUI;

    [Tooltip("This will only be triggered if the player enters a trigger collider with the 'EnterArea' tag.")]
    [SerializeField] Vector3 teleportTo = Vector3.zero;

    private SoundPlayer _soundPlayer;

    private bool _enteredArea;

    private Animator _anim;

    private void Start()
    {
        _soundPlayer = GetComponent<SoundPlayer>();
        _anim = transitionUI.GetComponent<Animator>();

        TransitionManager.onDarkenFinished += TransportPlayer;
        EventBus<LevelFinishedEvent>.OnEvent += FinishLevel;
        EventBus<GoUpScaffolding>.OnEvent += TriggerTransition;
    }

    private void OnDestroy()
    {
        TransitionManager.onDarkenFinished -= TransportPlayer;
        EventBus<LevelFinishedEvent>.OnEvent -= FinishLevel;
        EventBus<GoUpScaffolding>.OnEvent -= TriggerTransition;

        // In case it was subscribed as well...
        TransitionManager.onDarkenFinished -= BackToLobby;
    }

    private void TransportPlayer()
    {
        // This should set the XROrigin object to the desired position...
        EventBus<GoBackToStartPosEvent>.Publish(new GoBackToStartPosEvent());
        _anim.SetTrigger("LightenScreen");
        TriggerDialogueEvents();
    }

    private void FinishLevel(LevelFinishedEvent pLevelFinishEvent)
    {
        TransitionManager.onDarkenFinished -= TransportPlayer;
        TransitionManager.onDarkenFinished += PlaySound;

        StartCoroutine(LoadLobby(pLevelFinishEvent.waitTime));
    }

    private IEnumerator LoadLobby(float pWaitTime)
    {
        // If a the wait time is above 0 the screen will first darken and then play a sound if available...
        // This is useful for the fencing minigame...
        if (pWaitTime > 0)
        {
            _anim.SetTrigger("DarkenScreen");
        }

        yield return new WaitForSeconds(pWaitTime);

        if (pWaitTime == 0)
        {
            TransitionManager.onDarkenFinished += BackToLobby;

            _anim.SetTrigger("DarkenScreen");
        }
        else 
        {
            TransitionManager.onDarkenFinished -= PlaySound;
            BackToLobby(); 
        }
    }

    private void PlaySound()
    {
        if (_soundPlayer != null) { _soundPlayer.Play(); }
    }

    private void TriggerDialogueEvents()
    {
        // This should trigger all dialogue events...
        // Won't be a problem as the scene itself only listens to the important ones...
        EventBus<PaintTellAboutList>.Publish(new PaintTellAboutList());
    }

    private void BackToLobby()
    {
        GameManager.Instance.LoadSceneSpecific(4, false);
    }

    private void TriggerTransition(GoUpScaffolding pGoUpScaffolding)
    {
        _anim.SetTrigger("DarkenScreen");
    }
}
