using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarkerTexture : MonoBehaviour
{
    [SerializeField] ColorMatcher colorMatcher;

    [SerializeField] Transform tip;
    [SerializeField] int penSize = 5;

    [SerializeField] Transform penOrientation;

    private Rigidbody _rb;
    private Renderer _renderer;
    private Color[] _colors;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;
    private Quaternion _lastTouchRot;
    private bool _touchedLastFrame;


    void Start()
    {
        colorMatcher.Initialize();
        _renderer = tip.GetComponent<Renderer>();
        _rb = GetComponent<Rigidbody>();

        // Create an array of pixels with the same color the pen uses...
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(tip.position,  penOrientation.forward, out _touch, 0.04f))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }


                if (_rb.constraints != RigidbodyConstraints.FreezeRotation)
                {
                    _rb.angularVelocity = Vector3.zero;
                    _rb.constraints = RigidbodyConstraints.FreezeRotation;
                    transform.localRotation = Quaternion.Euler(90, 0, 0);
                    return;
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                // Calculate the touch position, based on the whiteboard's resolution...
                int x = (int)(_touchPos.x * _whiteboard.textureSize.x - (penSize / 2));
                int y = (int)(_touchPos.y * _whiteboard.textureSize.y - (penSize / 2));

                // Check if the pen is out of bounds...
                if (x < 0 || x > _whiteboard.textureSize.x ||
                    y < 0 || y > _whiteboard.textureSize.y)
                {
                    return;
                }

                if (_touchedLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, penSize, penSize, _colors);

                    // Interpolate the positions touched, to create a line following your pen's movement...
                    for (float f = 0.01f; f < 1.00f; f += 0.03f)
                    {
                        int lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        int lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, penSize, penSize, _colors);
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
        else if (_rb.constraints == RigidbodyConstraints.FreezeRotation)
        {
            _rb.constraints = RigidbodyConstraints.None;
        }

        // If no board was hit, reset the whiteboard reference...
        _whiteboard = null;
        _touchedLastFrame = false;
    }

    void Paint(int x, int y)
    {
        if (_whiteboard == null ) { return; }

        // Ensure the coordinates are within the bounds of the texture...
        Texture2D whiteboardTexture = _whiteboard.texture;

        if (IsWithinBounds(x, y, penSize, whiteboardTexture.width, whiteboardTexture.height))
        {
            whiteboardTexture.SetPixels(x, y, penSize, penSize, _colors);
            whiteboardTexture.Apply();
        }
    }

    bool IsWithinBounds(int x, int y, int penSize, int textureWidth, int textureHeight)
    {
        // Check if the starting point is within bounds...
        if (x < 0 || y < 0)
            return false;

        // Check if the rectangle exceeds the texture dimensions...
        if (x + penSize > textureWidth || y + penSize > textureHeight)
            return false;

        return true;
    }

    public void ChangeColor(ColorMatcher.Colors pColor)
    {
        _renderer.material = colorMatcher.GetBrushMaterial(pColor);
        Texture2D drawTexture = colorMatcher.GetDrawMaterial(pColor);

        if (drawTexture != null)
        {
            Color color = drawTexture.GetPixel(drawTexture.width / 2, drawTexture.height / 2);
            _colors = Enumerable.Repeat(color, penSize * penSize).ToArray();
        }
    }
}