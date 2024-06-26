using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLever : MonoBehaviour
{
    [SerializeField]
    int maxActiveLever = 90;
    [SerializeField]
    int minActiveLever = 60;
    void Update()
    {
       
        if((transform.rotation.eulerAngles.x > minActiveLever && transform.rotation.eulerAngles.x < maxActiveLever) || (transform.rotation.eulerAngles.x < 360-minActiveLever && transform.rotation.eulerAngles.x > 360-maxActiveLever) )
        {
            Debug.Log("dp left hand or right hand");
            //EventBus<LeverActivatedEvent>.Publish(new LeverActivatedEvent());
        }
    }
}
