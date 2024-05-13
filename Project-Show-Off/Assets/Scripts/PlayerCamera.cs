using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    Transform VRCamera;

    [SerializeField]
    bool falling;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!falling)
        {
            transform.position = VRCamera.transform.position;
        }
        else
        {
            transform.position = new Vector3(VRCamera.transform.position.x, transform.position.y - 1, VRCamera.transform.position.z);
        }
        
        transform.rotation = VRCamera.transform.rotation; 
    }
}
