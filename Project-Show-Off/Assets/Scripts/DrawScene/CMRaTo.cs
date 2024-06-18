using UnityEngine;

public class CMRaTo : MonoBehaviour
{
    [Tooltip("The position the rat will end up at after moving.")]
    [SerializeField] Vector3 movePos;
    [Tooltip("The rotation the rat will have after moving.")]
    [SerializeField] Vector3 moveRot;

    private void Start()
    {
        TransitionManager.onDarkenFinished += TransportRaTo;
    }

    private void OnDestroy()
    {
        TransitionManager.onDarkenFinished -= TransportRaTo;
    }

    private void TransportRaTo()
    {
        transform.position = movePos;
        transform.rotation = Quaternion.Euler(moveRot);
    }
}
