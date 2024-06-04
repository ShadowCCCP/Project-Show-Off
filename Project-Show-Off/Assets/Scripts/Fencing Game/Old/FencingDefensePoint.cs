using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencingDefensePoint : MonoBehaviour
{
    [SerializeField] Material defaultMaterial;
    [SerializeField] Material activeMaterial;

    private Renderer _renderer;
    private bool _active;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            _renderer.material = activeMaterial;
            _active = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            _renderer.material = defaultMaterial;
            _active = false;
        }
    }

    public void ResetState()
    {
        _active = false;
        _renderer.material = defaultMaterial;
    }

    public bool GetState()
    {
        return _active;
    }
}
