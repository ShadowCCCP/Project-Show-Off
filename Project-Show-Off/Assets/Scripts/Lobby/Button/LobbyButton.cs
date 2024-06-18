using System.Collections;
using UnityEngine;

public class LobbyButton : VRAbstractButton
{
    [SerializeField] int loadScene = 0;
    [SerializeField] float sceneTransitionTime = 3.0f;
    [SerializeField] Material litUpMaterial;
    Material normaMat;

    Renderer ren;

    private void Start()
    {
        
        ren = GetComponent<Renderer>();
        normaMat = ren.material;
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

    public override void OnButtonPress()
    {
        StartCoroutine(TransitionToScene());
    }

    public void SetMaterialLit()
    {
        ren.material = litUpMaterial;
        Debug.Log("lit");
    }

    public void SetMaterialUnLit()
    {
        ren.material = normaMat;
        Debug.Log("unlit");
    }
}
