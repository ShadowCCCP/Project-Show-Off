using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Recreates the same object at the same position as soon as it's destroyed.
/// This is used to respawn the plates used in the space minigame.
/// </summary>

public class ReInstantiate : MonoBehaviour
{
    GameObject _instantiateObject;
    Vector3 _startPosition;
    Quaternion _startRotation;

    private void Start()
    {
        _instantiateObject = gameObject;
        _startPosition = transform.position;
        _startRotation = transform.rotation;
    }

    private void OnDestroy()
    {
        if (SceneManager.GetActiveScene().isLoaded && _instantiateObject != null)
        {
            GameObject obj = Instantiate(_instantiateObject, _startPosition, _startRotation, null);
            ActivateGameObject(obj);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Destroy(gameObject);
        }
    }

    private void ActivateGameObject(GameObject pObj)
    {
        // Activate the GameObject itself
        pObj.SetActive(true);

        // Activate all components on this GameObject
        foreach (var component in pObj.GetComponents<Component>())
        {
            if (component is Behaviour behaviour)
            {
                behaviour.enabled = true;
            }
        }

        // Recursively activate all children
        foreach (Transform child in pObj.transform)
        {
            ActivateGameObject(child.gameObject);
        }
    }
}
