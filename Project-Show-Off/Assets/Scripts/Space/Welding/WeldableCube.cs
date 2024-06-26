using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class WeldableCube : MonoBehaviour
{
    float weldTime =1; 
    private float timePassed = 0;

    Renderer ren;
    bool welded;

    void Awake()
    {
        EventBus<SetUpWeldableSettingsEvent>.OnEvent += setup;
    }

    void OnDestroy()
    {
        EventBus<SetUpWeldableSettingsEvent>.OnEvent -= setup;
    }
    void Start()
    {
        ren = GetComponent<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BlowTorch>() != null)
        {
            weldTimer();
        }
       
    }
    private void weldTimer()
    {
        if (timePassed < weldTime)
        {
            timePassed += Time.deltaTime;
            ren.material.SetFloat("_Float", timePassed/weldTime); 
            //time up when 75% done
            if (timePassed >= (weldTime * 3 )/4 &&!welded)
            {
                EventBus<WeldCubeEvent>.Publish(new WeldCubeEvent(this));
                welded = true;
                Collider collider = GetComponent<Collider>();
                collider.enabled = false;
            }
        }
    }

    void setup(SetUpWeldableSettingsEvent setUpWeldableSettingsEvent)
    {
        weldTime = setUpWeldableSettingsEvent.weldTime;
    }
}
