using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "new Level")]
public class Level : ScriptableObject
{
    public string LevelName;
    public int LevelIndex;
    public Sprite Icon;
    public string Year;
}
