using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Allows metal plates to be placed 
/// </summary>
public class MetalPlaceHolder : MonoBehaviour
{
    Renderer ren;

    [SerializeField]
    Material materialWhenPlaced;

    SoundPlayer soundPlayer;
    private void Start()
    {
        ren = GetComponent<Renderer>();
        soundPlayer = GetComponent<SoundPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Metal")
        {
            ren.material = materialWhenPlaced;
            EventBus<SpawnWeldablesEvent>.Publish(new SpawnWeldablesEvent(this));

            Destroy(other.gameObject);

            if(soundPlayer)
            soundPlayer.Play();
        }
    }


}
