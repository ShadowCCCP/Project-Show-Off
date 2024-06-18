using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Transform transitionUI;

    [Tooltip("This will only be triggered if the player enters a trigger collider with the 'EnterArea' tag.")]
    [SerializeField] Vector3 teleportTo = Vector3.zero;

    private bool _enteredArea;

    private Animator _anim;

    private void Start()
    {
        _anim = transitionUI.GetComponent<Animator>();

        TransitionManager.onDarkenFinished += TransportPlayer;
        EventBus<LevelFinishedEvent>.OnEvent += FinishLevel;
    }

    private void OnDestroy()
    {
        TransitionManager.onDarkenFinished -= TransportPlayer;
        EventBus<LevelFinishedEvent>.OnEvent -= FinishLevel;

        // In case it was subscribed as well...
        TransitionManager.onDarkenFinished -= BackToLobby;
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the trigger collider is an enter area...
        if (!_enteredArea && other.CompareTag("EnterArea"))
        {
            _enteredArea = true;
            _anim.SetTrigger("DarkenScreen");
        }
    }

    private void TransportPlayer()
    {
        // This should set the XROrigin object to the desired position...
        transform.parent.position = teleportTo;
        _anim.SetTrigger("LightenScreen");
    }

    private void FinishLevel(LevelFinishedEvent pLevelFinishEvent)
    {
        TransitionManager.onDarkenFinished -= TransportPlayer;
        TransitionManager.onDarkenFinished += BackToLobby;

        _anim.SetTrigger("DarkenScreen");
    }

    private void BackToLobby()
    {
        GameManager.Instance.LoadSceneSpecific(4);
    }
}
