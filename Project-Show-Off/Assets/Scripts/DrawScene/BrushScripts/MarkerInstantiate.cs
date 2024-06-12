using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarkerInstantiate : MonoBehaviour
{
    // To keep track of all instantiated paint instances...
    private Dictionary<Vector3, GameObject> _existingPaint = new Dictionary<Vector3, GameObject>();

    [SerializeField] private Transform tip;
    [SerializeField] private GameObject paintPrefab;

    private Renderer _renderer;
    private float _tipHeight;

    private RaycastHit _touch;
    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;
    private Quaternion _lastTouchRot;
    private bool _touchedLastFrame;


    void Start()
    {
        _renderer = tip.GetComponent<Renderer>();

        _tipHeight = tip.localScale.y;
    }

    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(tip.position, transform.up, out _touch, _tipHeight))
        {
            if (_touch.transform.CompareTag("Whiteboard"))
            {

                _touchPos = new Vector2(_touch.point.x, _touch.point.y);

                if (_touchedLastFrame)
                {
                    // Check if paint already exists...
                    if (!_existingPaint.ContainsKey(_touchPos))
                    {
                        _existingPaint.Add(_touchPos, Instantiate(paintPrefab, _touchPos, Quaternion.identity, _touch.transform));
                    }

                    // Interpolate the positions touched, to create a line following your pen's movement...
                    for (float f = 0.01f; f < 1.00f; f += 0.03f)
                    {
                        float lerpX = Mathf.Lerp(_lastTouchPos.x, _touchPos.x, f);
                        float lerpY = Mathf.Lerp(_lastTouchPos.y, _touchPos.y, f);

                        if (!_existingPaint.ContainsKey(new Vector3(lerpX, lerpY)))
                        {
                            _existingPaint.Add(new Vector3(lerpX, lerpY),
                            Instantiate(paintPrefab, new Vector3(lerpX, lerpY), Quaternion.identity, _touch.transform));
                        }
                    }

                    // Keep the rotation with which you touched the whiteboard...
                    // This prevents the pen from being pushed sideways by physics when drawing...
                    transform.rotation = _lastTouchRot;
                }

                _lastTouchPos = new Vector2(_touchPos.x, _touchPos.y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;

                return;
            }
        }

        _touchedLastFrame = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("ColorPad"))
        {
            _renderer.material = collision.transform.GetComponent<Renderer>().material;
        }
    }
}
