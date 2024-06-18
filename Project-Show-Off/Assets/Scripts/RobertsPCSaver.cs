using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobertsPCSaver : MonoBehaviour
{
    [SerializeField] int maxFramerate = 60;

    void Awake()
    {
#if UNITY_EDITOR
        Debug.Log(Useful.GetHierarchy(transform) + "\nRobertsPCSaver script is active. Keep ur PC cool ;))");
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = maxFramerate;
        #endif
    }
}
