using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeldingManager : MonoBehaviour
{
    [SerializeField]
    GameObject weldableCubePrefab;


    [SerializeField]
    List<Transform> points = new List<Transform>();

    [SerializeField]
    List<int> weldableBreak = new List<int>();

    [SerializeField]
    float weldTime;

    [SerializeField]
    int dividerDistance = 10;

    int amountOfCubes = 0;

    List<WeldableCube> doneCubes = new List<WeldableCube>();



    void Awake()
    {
        EventBus<WeldCubeEvent>.OnEvent += weldCube;
    }

    void OnDestroy()
    {
        EventBus<WeldCubeEvent>.OnEvent -= weldCube;
    }
    void Start()
    {
        amountOfCubes = 0;
        spawnWeldables();
        EventBus<SetUpWeldableSettingsEvent>.Publish(new SetUpWeldableSettingsEvent(weldTime));

        
    }

    void spawnWeldables()
    {
        if (points.Count >= 2)
        {
            for (int i = 0; i < points.Count - 1; i++)
            {
                if (!weldableBreak.Contains(i))
                {
                    Vector3 currentPos = points[i].position;
                    Vector3 direction = points[i + 1].position - points[i].position;
                    direction = direction.normalized;

                    currentPos += direction / dividerDistance;

                    for (int j = 0; j < 50; j++) //max length to prevent a crash
                    {
                        if (checkMaxWeldables(direction, currentPos, points[i + 1].position))
                        {
                            WeldableCube obj =  Instantiate(weldableCubePrefab, currentPos, Quaternion.identity, this.transform).GetComponent<WeldableCube>();

                            obj.transform.LookAt(points[i + 1].position);
                            amountOfCubes++;
                            currentPos += direction / dividerDistance;
                        }
                    }
                }
            }

        }
        else
        {
            Debug.Log("No weldable points");
        }
    }

    bool checkMaxWeldables(Vector3 dir, Vector3 pointA, Vector3 pointB)
    {
        if (dir.y > 0 && pointA.y >= pointB.y)
        {
           
            return false; 
        }
        else if (dir.y < 0&& pointA.y <= pointB.y) 
        {
            return false;
        }
        
        if(dir.z >0 && pointA.z >= pointB.z)
        {
            return false;
        }
        else if (dir.z < 0 && pointA.z <= pointB.z) 
        {
            return false ;
        }

        if (dir.x > 0 && pointA.x >= pointB.x)
        {
            return false;
        }
        else if (dir.x < 0 && pointA.x <= pointB.x)
        {
            return false;
        }


        return true;
    }

    void weldCube(WeldCubeEvent weldCubeEvent)
    {
        doneCubes.Add(weldCubeEvent.cube);



        if (doneCubes.Count >= amountOfCubes)
        {
            Debug.Log("go bac to lobby");
            GameManager.Instance.LoadSceneSpecific(0);

        }
    }


}
