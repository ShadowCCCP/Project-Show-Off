using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationYEP : MonoBehaviour
{
    [SerializeField] float yRotation = 1;
    void Update()
    {
        gameObject.transform.Rotate(0,yRotation * Time.deltaTime *100,0);       
    }
}
