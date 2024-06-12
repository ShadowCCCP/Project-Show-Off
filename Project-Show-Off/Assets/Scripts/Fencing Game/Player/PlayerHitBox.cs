using UnityEngine;

public class PlayerHitBox : MonoBehaviour
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
    }

    private void OnDestroy()
    {
        TransitionManager.onDarkenFinished -= TransportPlayer;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_enteredArea && other.CompareTag("EnterArea"))
        {
            _enteredArea = true;
            _anim.SetTrigger("DarkenScreen");
        }
    }

    private void TransportPlayer()
    {
        transform.parent.position = teleportTo;
        _anim.SetTrigger("LightenScreen");
    }
}
