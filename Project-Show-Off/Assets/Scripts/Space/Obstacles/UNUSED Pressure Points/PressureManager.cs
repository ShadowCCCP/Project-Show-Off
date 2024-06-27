using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//pressure mechanic never used in the final product
public class PressureManager : MonoBehaviour
{
    [SerializeField]
    List<PressurePoint> pressurePoints = new List<PressurePoint>(); //choose rand point and push + warning
    void Start()
    {
        StartCoroutine(pressureRoutine());
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
