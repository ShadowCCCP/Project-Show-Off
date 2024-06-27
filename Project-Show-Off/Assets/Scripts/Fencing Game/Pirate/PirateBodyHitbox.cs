using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles toggling the body colliders of the pirate.
/// The side parameter also provides information about which side was hit for the ImprovedFencingEnemy script.
/// </summary>

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
