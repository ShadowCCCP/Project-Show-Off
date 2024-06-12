using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MetalPlaceHolder : MonoBehaviour
{
    Renderer ren;

    [SerializeField]
    Material materialWhenPlaced;
    private void Start()
    {
        ren = GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Metal")
        {
            ren.material = materialWhenPlaced;
            EventBus<SpawnWeldablesEvent>.Publish(new SpawnWeldablesEvent(this));

            Destroy(other.gameObject);
        }
    }


}
