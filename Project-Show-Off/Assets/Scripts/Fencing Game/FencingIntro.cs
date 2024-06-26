using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencingIntro : MonoBehaviour
{
    [SerializeField] GameObject piratePrefab;
    [SerializeField] Vector3 pirateSpawnPos;
    [SerializeField] Quaternion pirateRotation;

    private void Start()
    {
        EventBus<OnSwordPickupEvent>.OnEvent += QueuePirateSpawn;
    }

    private void QueuePirateSpawn(OnSwordPickupEvent pOnSwordPickupEvent)
    {
        Debug.Log("Starting Coroutine");
        StartCoroutine(SpawnPirate());
        Debug.Log("Done Starting");
    }

    private IEnumerator SpawnPirate()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("GawkToobb");
        Instantiate(piratePrefab, pirateSpawnPos, pirateRotation, null);
    }
}
