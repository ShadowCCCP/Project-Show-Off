using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Randomizes the sprite attached to the image component for the painting minigame.
/// Could also be used to randomize any other gameObject possessing an image component.
/// </summary>

public class ImageRandomizer : MonoBehaviour
{
    [SerializeField] Sprite[] imageVariants;

    private Image _img;

    private void Start()
    {
        _img = GetComponent<Image>();

        // Set random picture...
        if (_img != null && imageVariants.Length > 0)
        {
            int random = Random.Range(0, imageVariants.Length);
            _img.sprite = imageVariants[random];
        }
    }
}
