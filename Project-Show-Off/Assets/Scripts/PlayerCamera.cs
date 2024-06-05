using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Transform VRCamera;
    [SerializeField]
    Transform player;

    [SerializeField]
    bool falling;

    float startY;

    void Start()
    {
        startY = player.position.y;
    }

    void Update()
    {
        if(player.transform.position.y < startY)
        {
            falling = true;
        }

        if (!falling)
        {
            transform.position = VRCamera.transform.position;
        }
        else
        {
            transform.position = player.transform.position;
            
        }
        
        transform.rotation = VRCamera.transform.rotation; 
    }
}
