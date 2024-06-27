using UnityEngine;

public class BloodyHandler : MonoBehaviour
{
    /// <summary>
    /// Handles triggering the blood animation for the player UI.
    /// </summary>

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
