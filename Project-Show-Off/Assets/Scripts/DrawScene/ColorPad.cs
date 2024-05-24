using System.Collections.Generic;
using UnityEngine;

public class ColorPad : MonoBehaviour
{
    [SerializeField] ColorMatcher.Colors color;
    private Transform _lastBrushTouched;
    private MarkerTexture _markerScript;

    List<ContactPoint> contactPoints = new List<ContactPoint>();
 
    private void OnCollisionEnter(Collision collision)
    {
        bool valid = false;

        // Get all contactPoints to see who was hit exactly...
        contactPoints.Clear();
        collision.GetContacts(contactPoints);

        // Check if brushTip hit the color...
        foreach (ContactPoint c in contactPoints)
        {
            if (c.otherCollider.CompareTag("BrushTip"))
            {
                valid = true; 
            }
            // Continue if BrushTip is one of contactPoints...
            if (valid) continue;
        }

        // Get the markerScript and set the color/material of the brush...
        if (valid) 
        {
            _markerScript = collision.transform.GetComponent<MarkerTexture>();
            if (_markerScript != null)
            {
                if (_markerScript == null || _lastBrushTouched != collision.transform)
                {
                    _lastBrushTouched = collision.transform;
                }

                _markerScript.ChangeColor(color);
            }
        }
    }
}
