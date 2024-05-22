using UnityEngine;

public class MaterialHolder
{
    [SerializeField] Material material;

    public Material GetMaterial()
    {
        return material;
    }
}
