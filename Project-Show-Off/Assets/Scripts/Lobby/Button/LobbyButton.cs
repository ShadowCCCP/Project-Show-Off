using System.Collections;
using UnityEngine;

public class LobbyButton : VRAbstractButton
{
    [SerializeField] int loadScene = 0;
    [SerializeField] float sceneTransitionTime = 3.0f;
    [SerializeField] Material litUpMaterial;
    Material normaMat;

    Renderer ren;

    protected override void Initialize()
    {
        base.Initialize();

        ren = GetComponent<Renderer>();
        normaMat = ren.material;
    }

    private IEnumerator TransitionToScene()
    {
        // Load scene after a short pause for transitions to happen...
        yield return new WaitForSeconds(sceneTransitionTime);
        GameManager.Instance.LoadSceneSpecific(loadScene, false);
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
    }

    public void SetMaterialUnLit()
    {
        ren.material = normaMat;
    }
}
