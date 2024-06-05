using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateSwordHitbox : MonoBehaviour
{
    private CapsuleCollider _swordCol;
    private ImprovedFencingEnemy _impFE;

    private void Start()
    {
        _impFE = GetComponentInParent<ImprovedFencingEnemy>();
        _swordCol = GetComponent<CapsuleCollider>();

        _impFE.onToggleSwordCol += ToggleSwordCollider;
    }

    private void OnDestroy()
    {
        _impFE.onToggleSwordCol -= ToggleSwordCollider;
    }

    private void ToggleSwordCollider()
    {
        _swordCol.enabled = !_swordCol.enabled;
    }
}
