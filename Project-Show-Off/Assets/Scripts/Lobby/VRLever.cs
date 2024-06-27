using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// VR lever that pivots on one point
/// Never used in final product
/// </summary>

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
            //EventBus<LeverActivatedEvent>.Publish(new LeverActivatedEvent());
        }
    }
}
