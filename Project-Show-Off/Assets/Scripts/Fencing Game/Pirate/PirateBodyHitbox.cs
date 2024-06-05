using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateBodyHitbox : MonoBehaviour
{
    [SerializeField] ImprovedFencingEnemy.SideHit side;
    private BoxCollider _bodyCol;

    private ImprovedFencingEnemy _impFE;

    private void Start()
    {
        _impFE = GetComponentInParent<ImprovedFencingEnemy>();
        _bodyCol = GetComponent<BoxCollider>();

        _impFE.onToggleBodyCols += ToggleBodyCollider;
    }

    private void OnDestroy()
    {
        _impFE.onToggleBodyCols -= ToggleBodyCollider;
    }

    private void ToggleBodyCollider()
    {
        _bodyCol.enabled = !_bodyCol.enabled;
    }

    public void SideHit()
    {
        _impFE.SideGotHit(side);
    }
}
