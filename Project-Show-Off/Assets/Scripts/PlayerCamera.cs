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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < 0.5)
        {
            falling = true;
        }

        if (!falling)
        {
            transform.position = VRCamera.transform.position;
        }
        else
        {
            //transform.position = new Vector3(VRCamera.transform.position.x, transform.position.y - 1, VRCamera.transform.position.z);
            transform.position = player.transform.position;
            
        }
        
        transform.rotation = VRCamera.transform.rotation; 
    }
}
