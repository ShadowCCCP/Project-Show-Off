using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BrushTip : MonoBehaviour
{
    private MarkerTexture _markerScript;
    private Collider _lastHit;
    private ColorPad _colorPad;

    private void Start()
    {
        _markerScript = GetComponentInParent<MarkerTexture>();
    }

    private void Update()
    {
        if (transform.rotation != Quaternion.identity)
        {
            transform.localRotation = Quaternion.identity;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ColorPad"))
        {
            if (_lastHit == null || _lastHit != other)
            {
                _lastHit = other;
                _colorPad = other.GetComponent<ColorPad>();
            }

            _markerScript.ChangeColor(_colorPad.GetColor());
        }
    }
}
