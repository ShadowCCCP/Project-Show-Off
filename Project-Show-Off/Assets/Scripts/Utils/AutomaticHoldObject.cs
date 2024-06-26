using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticHoldObject : MonoBehaviour
{
    [SerializeField] Transform handPosition;

    private void Start()
    {
        transform.position = handPosition.position;
    }
}
