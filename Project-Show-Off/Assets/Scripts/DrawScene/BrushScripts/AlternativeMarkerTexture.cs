using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarkerTextureAlternative : MonoBehaviour
{
    [SerializeField] ColorMatcher colorMatcher;

    [SerializeField] Transform tip;
    [SerializeField] int penSize = 40;

    private Renderer _renderer;
    private Color[] _colors;

    private List<RaycastHit> _touch = new List<RaycastHit>();
    private Vector2 _touchPos;
    private Whiteboard _whiteboard;

    private Vector2 _lastTouchPos;
    private Quaternion _lastTouchRot;
    private bool _touchedLastFrame;


    void Start()
    {
        colorMatcher.Initialize();
        _renderer = tip.GetComponent<Renderer>();

        // Create an array of pixels with the same color the pen uses...
        _colors = Enumerable.Repeat(_renderer.material.color, penSize * penSize).ToArray();
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (CheckRaycast())
        {
            if (_touch[0].transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch[0].transform.GetComponent<Whiteboard>();
                }

                for (int i = 0; i < _touch.Count; i++)
                {
                    _touchPos = new Vector2(_touch[i].textureCoord.x, _touch[i].textureCoord.y);

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
        }

        // If no board was hit, reset the whiteboard reference...
        _whiteboard = null;
        _touchedLastFrame = false;
        
    }

    private bool CheckRaycast()
    {
        RaycastHit hit;
        // Reset the hit list...
        _touch.Clear();

        // Check straight up...
        Debug.DrawRay(tip.position, transform.up, Color.red);
        if (Physics.Raycast(tip.position, transform.up, out hit, 0.04f))
        {
            _touch.Add(hit);
        }

        // Check side angles...
        for (int i = 0; i < 360; i += 45)
        {
            Vector3 angledDirection = transform.rotation * Quaternion.AngleAxis(i, Vector3.up) * (Vector3.right + Vector3.up);
            Debug.DrawRay(tip.position, angledDirection);
            if (Physics.Raycast(tip.position, angledDirection, out hit, 0.04f))
            {
                _touch.Add(hit);
            }

            // And side way raycasts too...
            Vector3 sideDirection = transform.rotation * Quaternion.AngleAxis(i, Vector3.up) * Vector3.right;
            Debug.DrawRay(tip.position, sideDirection, Color.blue);
            if (Physics.Raycast(tip.position, transform.up, out hit, 0.04f))
            {
                _touch.Add(hit);
            }
        }

        if (_touch.Count > 0) { return true; }
        else { return false; }
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
