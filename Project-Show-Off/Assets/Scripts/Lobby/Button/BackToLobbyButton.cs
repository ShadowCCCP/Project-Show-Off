using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToLobbyButton : VRAbstractButton
{
    public override void OnButtonPress()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadSceneSpecific(5, false);
        }
    }
}
