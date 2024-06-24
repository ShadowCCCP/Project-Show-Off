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
        StartCoroutine(SpawnPirate());
    }

    private IEnumerator SpawnPirate()
    {
        yield return new WaitForSeconds(3);

        Instantiate(piratePrefab, pirateSpawnPos, pirateRotation, null);
    }
}
