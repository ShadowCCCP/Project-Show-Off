using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReInstantiate : MonoBehaviour
{
    GameObject _instantiateObject;
    Vector3 _startPosition;
    Quaternion _startRotation;

    private void Start()
    {
        _instantiateObject = gameObject;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    private void OnDestroy()
    {
        Instantiate(_instantiateObject, _startPosition, _startRotation);
    }
}
