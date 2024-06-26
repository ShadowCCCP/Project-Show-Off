using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "New Level")]
public class Level : ScriptableObject
{
    public string LevelName;
    public int LevelIndex;
    public Sprite Icon;
    public string Year;
    public string Danger;
    public string Hand;
    public string BigText;
}
