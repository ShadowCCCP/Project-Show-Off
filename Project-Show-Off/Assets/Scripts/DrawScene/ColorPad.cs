using UnityEngine;

public class ColorPad : MonoBehaviour
{
    [SerializeField] ColorMatcher.Colors color;
    private Transform _lastBrushTouched;
    private MarkerTexture _markerScript;
 
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Detected");
        if (collision.transform.CompareTag("BrushTip"))
        {
            Debug.Log("Brush!!!");

            if (_markerScript == null || _lastBrushTouched != collision.transform)
            {
                _lastBrushTouched = collision.transform;
                _markerScript = _lastBrushTouched.parent.GetComponent<MarkerTexture>();
            }

            _markerScript.ChangeColor(color);
        }
    }
}
