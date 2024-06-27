using UnityEngine;

/// <summary>
/// Creates the texture which will be painted on during the drawing minigame.
/// The bigger the size, the more perfomance it will take to actually paint on it.
/// </summary>

public class Whiteboard : MonoBehaviour
{
    [HideInInspector] public Texture2D texture;

    [Tooltip("The textureSize should have the same ratio as the actual object.")]
    public Vector2 textureSize = new Vector2(2048, 2048);

    private void Start()
    {
        Renderer r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }
}
