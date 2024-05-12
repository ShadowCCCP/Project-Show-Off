using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    [HideInInspector] public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);

    private void Start()
    {
        Renderer r = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        r.material.mainTexture = texture;
    }
}
