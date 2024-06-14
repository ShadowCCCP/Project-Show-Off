using UnityEngine;

public class BrushTip : MonoBehaviour
{
    private MarkerTextureAlternative _markerScript;
    private Collider _lastHit;
    private ColorPad _colorPad;
    private Vector3 _startPosition;

    private void Start()
    {
        _markerScript = GetComponentInParent<MarkerTextureAlternative>();
        _startPosition = transform.localPosition;
    }

    private void Update()
    {
        if (transform.rotation != Quaternion.identity)
        {
            transform.localRotation = Quaternion.identity;
        }

        if (transform.localPosition != _startPosition)
        {
            transform.localPosition = _startPosition;
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

            _markerScript.InstantiateSplash(transform.position, _colorPad.GetColor());
            _markerScript.ChangeColor(_colorPad.GetColor());
        }
    }
}
