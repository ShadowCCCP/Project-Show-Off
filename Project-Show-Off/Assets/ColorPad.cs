using UnityEngine;

public class ColorPad : MonoBehaviour
{
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.tag);

        if (collision.transform.CompareTag("MarkerTip"))
        {
            collision.transform.GetComponent<Renderer>().material = _renderer.material;
        }
    }
}
