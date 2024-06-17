using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarkerTextureAlternative : MonoBehaviour
{
    [SerializeField] bool slimPaintForm;

    [SerializeField] ColorMatcher colorMatcher;

    [SerializeField] Transform tip;
    [SerializeField] int penSize = 40;

    [SerializeField] GameObject paintSplashVFX;

    private Renderer _renderer;
    private Color[] _colors;

    private List<RaycastHit> _touch = new List<RaycastHit>();
    private Vector2 _touchPos;
    private Whiteboard _whiteboard;

    private Vector2 _lastTouchPos;
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

                // We only want to draw on the very first hit raycast, in order to save performance...
                _touchPos = new Vector2(_touch[0].textureCoord.x, _touch[0].textureCoord.y);

                // Calculate the touch position, based on the whiteboard's resolution...
                int x = (int)(_touchPos.x * _whiteboard.textureSize.x);
                int y = (int)(_touchPos.y * _whiteboard.textureSize.y);

                // Check if the pen is out of bounds...
                if (x < 0 || x > _whiteboard.textureSize.x ||
                    y < 0 || y > _whiteboard.textureSize.y)
                {
                    return;
                }

                if (_touchedLastFrame)
                {
                    if (!slimPaintForm) { SetPixelsInCircle(_whiteboard.texture, x, y, penSize, _colors); }
                    else { SetPixelsInEllipse(_whiteboard.texture, x, y, penSize, 0.5f, _colors); }

                    // Interpolate the positions touched, to create a line following your pen's movement...
                    for (float f = 0.01f; f < 1.00f; f += 0.03f)
                    {
                        int lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        int lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);

                        if (!slimPaintForm) { SetPixelsInCircle(_whiteboard.texture, lerpX, lerpY, penSize, _colors); }
                        else { SetPixelsInEllipse(_whiteboard.texture, lerpX, lerpY, penSize, 0.5f, _colors); }
                    }

                    _whiteboard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _touchedLastFrame = true;

                return;
            }
        }

        if (_touch.Count == 0 && (_whiteboard != null || _touchedLastFrame))
        {
            // If no board was hit, reset the whiteboard reference...
            _whiteboard = null;
            _touchedLastFrame = false;
        }
        
    }

    void SetPixelsInCircle(Texture2D pTexture, int pX, int pY, int pRadius, Color[] pColors)
    {
        int rSquared = pRadius * pRadius;
        int colorIndex = 0;

        for (int u = -pRadius; u <= pRadius; u++)
        {
            for (int v = -pRadius; v <= pRadius; v++)
            {
                if (u * u + v * v <= rSquared)
                {
                    int xPos = pX + u;
                    int yPos = pY + v;

                    if (xPos >= 0 && xPos < pTexture.width && yPos >= 0 && yPos < pTexture.height)
                    {
                        pTexture.SetPixel(xPos, yPos, pColors[colorIndex % pColors.Length]);
                    }
                    colorIndex++;
                }
            }
        }
    }

    void SetPixelsInEllipse(Texture2D pTexture, int pX, int pY, int pRadius, float squashFactor, Color[] pColors)
    {
        int xRadius = (int)(pRadius * squashFactor);
        int yRadius = pRadius;
        int colorIndex = 0;

        for (int u = -xRadius; u <= xRadius; u++)
        {
            for (int v = -yRadius; v <= yRadius; v++)
            {
                // Check if the point is inside the ellipse
                if ((u * u) / (float)(xRadius * xRadius) + (v * v) / (float)(yRadius * yRadius) <= 1.0f)
                {
                    int xPos = pX + u;
                    int yPos = pY + v;

                    if (xPos >= 0 && xPos < pTexture.width && yPos >= 0 && yPos < pTexture.height)
                    {
                        pTexture.SetPixel(xPos, yPos, pColors[colorIndex % pColors.Length]);
                    }
                    colorIndex++;
                }
            }
        }
    }

    private bool CheckRaycast()
    {
        RaycastHit hit;
        // Reset the hit list...
        _touch.Clear();

        // Check straight...
        Debug.DrawRay(tip.position, transform.forward, Color.red);
        if (Physics.Raycast(tip.position, transform.forward, out hit, 0.06f))
        {
            _touch.Add(hit);
        }

        // Check side angles...
        for (int i = 0; i < 360; i += 45)
        {
            Vector3 angledDirection = transform.rotation * Quaternion.AngleAxis(i, Vector3.forward) * (Vector3.right + Vector3.forward);
            Debug.DrawRay(tip.position, angledDirection);
            if (Physics.Raycast(tip.position, angledDirection, out hit, 0.06f))
            {
                _touch.Add(hit);
            }

            Vector3 sideDirection = transform.rotation * Quaternion.AngleAxis(i, Vector3.forward) * Vector3.right;
            Debug.DrawRay(tip.position, sideDirection, Color.blue);
            if (Physics.Raycast(tip.position, transform.forward, out hit, 0.06f))
            {
                _touch.Add(hit);
            }
        }

        if (_touch.Count > 0) { return true; }
        else { return false; }
    }

    private Color GetColorOfTexture(Texture2D pTexture)
    {
        return pTexture.GetPixel(pTexture.width / 2, pTexture.height / 2);
    }

    public void ChangeColor(ColorMatcher.Colors pColor)
    {
        _renderer.material = colorMatcher.GetBrushMaterial(pColor);
        Texture2D drawTexture = colorMatcher.GetDrawMaterial(pColor);

        if (drawTexture != null)
        {
            Color color = GetColorOfTexture(drawTexture);
            _colors = Enumerable.Repeat(color, penSize * penSize).ToArray();
        }
    }

    public void InstantiateSplash(Vector3 pPos, ColorMatcher.Colors pColor)
    {
        Color color = GetColorOfTexture(colorMatcher.GetDrawMaterial(pColor));

        if (color != _colors[0])
        {
            GameObject colorSplashObj = Instantiate(paintSplashVFX, pPos, Quaternion.identity, null);
            ParticleSystem cSplash = colorSplashObj.GetComponent<ParticleSystem>();
            /*
            ParticleSystem.MainModule psmain = cSplash.main;
            psmain.startColor = gameObject.GetComponent<SpriteRenderer>().color;
            */
            cSplash.startColor = color;
        }
    }
}
