using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionButton : VRAbstractButton
{
    [SerializeField] Transform transitionUI;
    [SerializeField] float timeBeforeTransition = 5;

    private Animator _anim;

    protected override void Initialize()
    {
        base.Initialize();

        if (transitionUI == null)
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nTransitionButton: No transitionUI attached to the script.");
            return;
        }
        _anim = transitionUI.GetComponent<Animator>();
    }

    public override void OnButtonPress()
    {
        TriggerEvent();
        StartCoroutine(Transition());
    }

    private void TriggerEvent()
    {
        EventBus<OnPencilPickupEvent>.Publish(new OnPencilPickupEvent());
    }

    private IEnumerator Transition()
    {
        yield return new WaitForSeconds(timeBeforeTransition);
        _anim.SetTrigger("DarkenScreen");
    }
}
