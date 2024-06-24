using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUntilTransition : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DelayedStart());
        TransitionManager.onDarkenFinished += ActivateChildren;
    }

    private void OnDestroy()
    {
        TransitionManager.onDarkenFinished -= ActivateChildren;
    }

    private void HideChildren()
    {
        // Deactivate all children of this transform...
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void ActivateChildren()
    {
        // Activate all children of this transform...
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private IEnumerator DelayedStart()
    {
        // This fixes the issue of deactivated things not getting to do their Start() setup...
        yield return new WaitForSeconds(0.1f);
        HideChildren();
    }
}