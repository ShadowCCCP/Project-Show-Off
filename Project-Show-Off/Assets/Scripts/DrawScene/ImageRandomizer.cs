using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
