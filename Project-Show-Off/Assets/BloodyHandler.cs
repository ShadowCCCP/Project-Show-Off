using UnityEngine;

public class BloodyHandler : MonoBehaviour
{
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
        EventBus<OnPlayerHitEvent>.OnEvent += ToggleBloody;
    }

    private void OnDestroy()
    {
        EventBus<OnPlayerHitEvent>.OnEvent -= ToggleBloody;
    }

    private void ToggleBloody(OnPlayerHitEvent pOnPlayerHitEvent)
    {
        _anim.SetTrigger("Bloody");
    }
}
