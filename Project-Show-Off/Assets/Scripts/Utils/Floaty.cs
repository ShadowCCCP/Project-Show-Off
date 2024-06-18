using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Floaty : MonoBehaviour
{
    [SerializeField] float floatDistance = 0.2f;
    [SerializeField] float timePerFloat = 2.0f;

    private int _tweenId;
    private Vector3 _initialPosition;

    private XRGrabInteractable _gInteractable;

    private void Start()
    {
        _initialPosition = transform.position;
        StartFloating();

        // In order to be able to deactivate floating when the object is interacted with (grabbed)...
        _gInteractable = GetComponent<XRGrabInteractable>();
        if (_gInteractable != null )
        {
            _gInteractable.selectEntered.AddListener(CancelTween);
        }
    }

    private void OnDestroy()
    {
        if (_gInteractable != null)
        {
            _gInteractable.selectEntered.RemoveListener(CancelTween);
        }
    }

    private void StartFloating()
    {
        _tweenId = LeanTween.moveY(gameObject, _initialPosition.y + floatDistance, timePerFloat).setEaseInOutSine().setLoopPingPong().id;
    }

    public void CancelTween(SelectEnterEventArgs args)
    {
        LeanTween.cancel(_tweenId);
    }

    public void StartTween()
    {
        if (_gInteractable == null) return;

        _initialPosition = transform.position;
        StartFloating();
    }
}
