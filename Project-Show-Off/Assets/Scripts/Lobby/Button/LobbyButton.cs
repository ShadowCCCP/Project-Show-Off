using System.Collections;
using UnityEngine;

public class LobbyButton : VRAbstractButton
{
    [SerializeField] int loadScene = 0;
    [SerializeField] float sceneTransitionTime = 3.0f;

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

    public override void OnButtonPress()
    {
        Debug.Log("da");
        StartCoroutine(TransitionToScene());
    }
}
