using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PointHolder 
{
    public List<Transform> points;
    public MetalPlaceHolder metalPlatePlace;

    [HideInInspector]
    public bool Welded;
    [HideInInspector]
    public int weldablesAmount;
}
