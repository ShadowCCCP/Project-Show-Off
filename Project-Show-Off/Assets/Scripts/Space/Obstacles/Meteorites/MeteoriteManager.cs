using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteManager : MonoBehaviour
{
    [SerializeField]
    GameObject meteoritePrefab;
    [SerializeField]
    List<Transform> spawnPoints = new List<Transform>();

    [SerializeField]
    float timeBetweenSpawns = 2;
    [SerializeField]
    Vector3 direction;
    [SerializeField]
    int speed;

    [SerializeField]
    float delayFirstSpawn;
    void Start()
    {
        //StartSpawning();
    }
    IEnumerator meteoriteRoutine()
    {
        int r = Random.Range(0, spawnPoints.Count);

        //spawn meteorite
        SpawnMeteorite(spawnPoints[r]);
        yield return new WaitForSeconds(timeBetweenSpawns);

        //restart routine
        StartCoroutine(meteoriteRoutine());
    }

    void SpawnMeteorite(Transform transf)
    {
        GameObject meteor = Instantiate(meteoritePrefab, transf.position, Quaternion.identity, this.transform);

        //set speed and direction
        meteor.GetComponent<Meteorite>().speed = speed;
        meteor.GetComponent<Meteorite>().direction = direction;
    }

    bool doItOnce;

    public void StartSpawning()
    {
        if (!doItOnce)
        {
            StartCoroutine(spawningDelay());
            doItOnce = true;
        }
    }

    IEnumerator spawningDelay()
    {
        yield return new WaitForSeconds(delayFirstSpawn);
        StartCoroutine(meteoriteRoutine());
    }
}
