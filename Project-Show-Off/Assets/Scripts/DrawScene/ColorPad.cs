using UnityEngine;

public class ColorPad : MonoBehaviour
{
    [SerializeField] ColorMatcher.Colors color;

    public ColorMatcher.Colors GetColor()
    {
        return color;
    }
}
