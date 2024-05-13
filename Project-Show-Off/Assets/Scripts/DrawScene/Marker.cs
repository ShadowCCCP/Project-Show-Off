using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;
    private Quaternion _lastTouchRot;
    private bool _touchedLastFrame;


    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();

        // Create an array of pixels with the same color the pen uses...
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();

        _tipHeight = _tip.localScale.y;
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                // Calculate the touch position, based on the whiteboard's resolution...
                int x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                int y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                // Check if the pen is out of bounds...
                if (x < 0 || x > _whiteboard.textureSize.x ||
                    y < 0 || y > _whiteboard.textureSize.y)
                {
                    return;
                }

                if (_touchedLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, _colors);

                    // Interpolate the positions touched, to create a line following your pen's movement...
                    for (float f = 0.01f; f < 1.00f; f += 0.03f)
                    {
                        int lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        int lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                    }

                    // Keep the rotation with which you touched the whiteboard...
                    // This prevents the pen from being pushed sideways by physics when drawing...
                    transform.rotation = _lastTouchRot;

                    _whiteboard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;

                return;
            }
        }

        // If no board was hit, reset the whiteboard reference...
        _whiteboard = null;
        _touchedLastFrame = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ColorPad"))
        {
            Material m = collision.transform.GetComponent<Renderer>().material;
            if (m.name == "ColorPalette") { return; }

            _renderer.material = m;
        }
    }
}
