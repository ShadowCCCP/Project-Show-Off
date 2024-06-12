using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    [SerializeField] int targetRotation = 180;
    [SerializeField] int secondsToPaint = 30;
    float totalRotation;

    
    void Update()
    {
        var rotation = targetRotation/secondsToPaint * Time.deltaTime;
        gameObject.transform.Rotate(0, rotation, 0);
        totalRotation += rotation;

        if(totalRotation >= 180)
        {
            this.enabled = false;
        }       
    }
}
