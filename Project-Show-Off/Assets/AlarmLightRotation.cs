using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLightRotation : MonoBehaviour
{
    [SerializeField] float yRotationspeed = 1;
    void Update()
    {
        gameObject.transform.Rotate(0,yRotationspeed * Time.deltaTime *100,0);       
    }
}
