using System.Collections;
using UnityEngine;

// TODO
// Use leanTween to animate the button tap, as well as the glass opening...

public class LobbyButton : MonoBehaviour
{
    [SerializeField] int loadScene = 0;
    [SerializeField] float sceneTransitionTime = 3.0f;

    [SerializeField] float buttonPressDepth = 1.0f;
    [SerializeField] float buttonPressDuration = 0.5f;
    [SerializeField] float buttonResetDuration = 0.3f;

    [SerializeField] Transform glass;
    
    private Vector3 _buttonStartPos;

    private bool _glassOpen;

    private void Start()
    {
        // Set the buttonStartPos to transform.position, as it's attached to the button...
        _buttonStartPos = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Controller" && _glassOpen)
        {
            // Move the button back...
            TweenButton();
        }
    }

    private void TweenButton()
    {
        LeanTween.move(gameObject, _buttonStartPos - new Vector3(0, 0, buttonPressDepth), buttonPressDuration).setOnComplete(() =>
        {
            // Move back to the original position
            LeanTween.move(gameObject, _buttonStartPos, buttonResetDuration);
        });
    }

    private void TweenGlass()
    {

    }

    private IEnumerator TransitionToScene()
    {
        // Load scene after a short pause for transitions to happen...
        yield return new WaitForSeconds(sceneTransitionTime);
        GameManager.Instance.LoadSceneSpecific(loadScene);
    }    

    public void SetToGoScene(int pIndex)
    {
        loadScene = pIndex;
    }
}
