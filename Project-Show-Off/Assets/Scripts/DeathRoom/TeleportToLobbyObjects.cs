using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used in the in between scene  
/// </summary>

public class TeleportToLobbyObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.LoadSceneSpecific(5, false);
            }
            else
            {
                Debug.LogError("No GameManager");
            }
        }
    }
}
