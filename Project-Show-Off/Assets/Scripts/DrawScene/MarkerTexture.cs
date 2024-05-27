using System.Linq;
using UnityEngine;

// TODO
// Only try to implement this if much time is left:
// Change the script so that it gets the collision points and paints at those points of the texture instead...
// (This has the advantage of me not manually having to straighten the rotation and fix the position)

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
    private float _touchAxisZ;


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
        Debug.DrawRay(tip.position, -penOrientation.up, Color.red);
        if (Physics.Raycast(tip.position, -penOrientation.up, out _touch, 0.04f))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }


                if (_rb.constraints != RigidbodyConstraints.FreezeRotation)
                {
                    // Set the position to the collision point...
                    transform.position = new Vector3(_touch.point.x, _touch.point.y ,transform.position.z);
                    _touchAxisZ = transform.position.z;

                    // Stop any angular movement happening and straighten the pen...
                    // This makes the pen feel more precise and be less stuttery...
                    _rb.angularVelocity = Vector3.zero;
                    _rb.constraints = RigidbodyConstraints.FreezeRotation;
                    transform.localRotation = Quaternion.Euler(0, 0, 0);
                    return;
                }

                // Reset x axis to not let the pen go inside the whiteboard...
                if (transform.position.z > _touchAxisZ)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, _touchAxisZ);
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