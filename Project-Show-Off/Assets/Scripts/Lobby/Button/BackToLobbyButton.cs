using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLobbyButton : VRAbstractButton
{
    private bool _loadingLevel;

    public override void OnButtonPress()
    {
        if (!_loadingLevel && GameManager.Instance != null)
        {
            GameManager.Instance.LoadSceneSpecific(5, false);
            _loadingLevel = true;
        }
    }
}
