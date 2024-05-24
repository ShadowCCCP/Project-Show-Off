using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSelection : MonoBehaviour
{
    //gamemanager singleton 
    [SerializeField]
    bool rightHandHole = false;


    private void OnTriggerEnter(Collider other)
    {
        if ((other.tag == "Controller"))
        {
            if (rightHandHole)
            {
                Debug.Log("Change to right hand");
            }
            else
            {
                Debug.Log("Change to left hand");
            }
        }
    }
}
