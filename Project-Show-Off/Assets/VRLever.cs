using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLever : MonoBehaviour
{
    [SerializeField]
    bool leverActvated = false;
    [SerializeField]
    int maxActiveLever = 90;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.rotation.eulerAngles.x);
        if((transform.rotation.eulerAngles.x > 70 && transform.rotation.eulerAngles.x < 90) || (transform.rotation.eulerAngles.x < -70 && transform.rotation.eulerAngles.x > -90) )
        {

        }
    }
}
