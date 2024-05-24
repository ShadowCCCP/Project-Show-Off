using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct ColorMatcher
{
    public enum Colors { Red, Green, Yellow, Blue, Purple }

    [SerializeField] List<Colors> colors;
    [SerializeField] List<Material> brushTipMaterials;
    [SerializeField] List<Texture2D> drawTextures;

    private Dictionary<Colors, Material> brushColorDict;
    private Dictionary<Colors, Texture2D> drawColorDict;

    public void Initialize()
    {
        brushColorDict = new Dictionary<Colors, Material>();
        drawColorDict = new Dictionary<Colors, Texture2D>();

        // Check if even amount of colors and materials inserted in lists...
        if (colors.Count == brushTipMaterials.Count && colors.Count == drawTextures.Count)
        {
            for (int i = 0; i < colors.Count; i++)
            {
                brushColorDict.Add(colors[i], brushTipMaterials[i]);
                drawColorDict.Add(colors[i], drawTextures[i]);
            }
        }
        else
        {
            Debug.LogError("ColorMatcher: Uneven amount of colors and materials.");
        }
    }

    // Use dictionary to return materials...
    public Material GetBrushMaterial(Colors pColor)
    {
        return brushColorDict[pColor];
    }

    public Texture2D GetDrawMaterial(Colors pColor)
    {
        return drawColorDict[pColor];
    }
}
