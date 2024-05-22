using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct ColorMatcher
{
    public enum Colors { Red, Green, Yellow, Blue, Purple }

    [SerializeField] List<Colors> colors;
    [SerializeField] List<Material> materials;

    private Dictionary<Colors, Material> colorMatchDict;

    public ColorMatcher(bool pUseless = true)
    {
        colors = new List<Colors>();
        materials = new List<Material>();

        colorMatchDict = new Dictionary<Colors, Material>();

        // Check if even amount of colors and materials inserted in lists...
        if (colors.Count == materials.Count)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                colorMatchDict.Add(colors[i], materials[i]);
            }
        }
        else
        {
            Debug.LogError("ColorMatcher: Uneven amount of colors and materials.");
        }
    }

    public Material GetMaterial(Colors pColor)
    {
        // Use dictionary to return materials...
        return colorMatchDict[pColor];
    }
}
