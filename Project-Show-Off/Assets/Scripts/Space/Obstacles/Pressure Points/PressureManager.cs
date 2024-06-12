using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureManager : MonoBehaviour
{
    [SerializeField]
    List<PressurePoint> pressurePoints = new List<PressurePoint>(); //choose rand point and push + warning
    void Start()
    {
        StartCoroutine(pressureRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator pressureRoutine()
    {
        int r = Random.Range(0, pressurePoints.Count);

        //warning smoke
        pressurePoints[r].OnWarning();
        yield return new WaitForSeconds(2);

        //push smoke
        pressurePoints[r].OnPush();
        yield return new WaitForSeconds(2);

        //push back
        pressurePoints[r].Retreat();
        yield return new WaitForSeconds(2);

        //restart routine
        StartCoroutine(pressureRoutine());
    }

}
