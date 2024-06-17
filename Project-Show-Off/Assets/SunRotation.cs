using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    [SerializeField] int targetRotation = 180;
    [SerializeField] int secondsToPaint = 30;
    float totalRotation;

    private void Start()
    {
        
    }


    void Update()
    {
        var rotation = (float)targetRotation/(float)secondsToPaint * Time.deltaTime;
        gameObject.transform.Rotate(0, rotation, 0);
        totalRotation += rotation;

        if(totalRotation >= targetRotation)
        {
            this.enabled = false;
        }       
    }
}
