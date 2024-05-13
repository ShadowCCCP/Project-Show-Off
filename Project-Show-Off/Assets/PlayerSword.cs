using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] Transform handPosition;

    private void Start()
    {
        transform.position = handPosition.position;
    }
}
