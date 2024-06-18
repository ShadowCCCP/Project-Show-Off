using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Floaty : MonoBehaviour
{
    [SerializeField] float floatDistance = 0.2f;
    [SerializeField] float timePerFloat = 2.0f;

    private int tweenId;
    private Vector3 initialPosition;

    private XRGrabInteractable gInteractable;

    private void Start()
    {
        initialPosition = transform.position;
        StartFloating();

        // In order to be able to deactivate floating when the object is interacted with (grabbed)...
        gInteractable = GetComponent<XRGrabInteractable>();
        if (gInteractable != null )
        {
            gInteractable.selectEntered.AddListener(PauseTween);
        }
    }

    private void OnDestroy()
    {
        gInteractable.selectEntered.RemoveListener(PauseTween);
    }

    private void StartFloating()
    {
        tweenId = LeanTween.moveY(gameObject, initialPosition.y + floatDistance, timePerFloat).setEaseInOutSine().setLoopPingPong().id;
    }

    private void PauseTween(SelectEnterEventArgs args)
    {
        LeanTween.pause(tweenId);
    }

    private void ResumeTween()
    {
        LeanTween.resume(tweenId);
    }
}
