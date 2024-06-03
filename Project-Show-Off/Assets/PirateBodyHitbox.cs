using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PirateBodyHitbox : MonoBehaviour
{
    [SerializeField] ImprovedFencingEnemy.SideHit side;

    private ImprovedFencingEnemy _impFE;

    private void Start()
    {
        _impFE = FindObjectOfType<ImprovedFencingEnemy>();
    }

    public void SideHit()
    {
        _impFE.SideGotHit(side);
    }
}
