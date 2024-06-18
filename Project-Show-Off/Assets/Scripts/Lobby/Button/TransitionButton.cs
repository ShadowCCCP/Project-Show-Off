using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionButton : VRAbstractButton
{
    [SerializeField] Transform transitionUI;
    private Animator _anim;

    private void Start()
    {
        if (transitionUI == null)
        {
            Debug.LogError(Useful.GetHierarchy(transform) + "\nTransitionButton: No transitionUI attached to the script.");
            return;
        }
        _anim = transitionUI.GetComponent<Animator>();
    }

    public override void OnButtonPress()
    {
        _anim.SetTrigger("DarkenScreen");
    }
}
