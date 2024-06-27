using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Removes glass shards after animation
/// </summary>
public class GlassBreaking : MonoBehaviour
{
    public void RemoveGlass()
    {
        gameObject.SetActive(false);
    }
}
