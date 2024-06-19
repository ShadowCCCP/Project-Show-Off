using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeldingManager : MonoBehaviour
{
    [SerializeField]
    GameObject weldableCubePrefab;




    [SerializeField]
    float weldTime;

    [SerializeField]
    int dividerDistance = 10;

    List<WeldableCube> doneCubes = new List<WeldableCube>();

    [SerializeField]
    List<PointHolder> pointHolders = new List<PointHolder>();

    int amountOfCubes = 0;
    int platesSpawned = 0;

    void Awake()
    {
        EventBus<WeldCubeEvent>.OnEvent += weldCube;
        EventBus<SpawnWeldablesEvent>.OnEvent += FindThePointsForPlate;
    }

    void OnDestroy()
    {
        EventBus<WeldCubeEvent>.OnEvent -= weldCube;
        EventBus<SpawnWeldablesEvent>.OnEvent -= FindThePointsForPlate;
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



        if (doneCubes.Count >= amountOfCubes && platesSpawned == pointHolders.Count)
        {
            Debug.Log("Game Finished, goig back to lobby");
            GameManager.Instance.LoadSceneSpecific(5);

        }
    }

    bool onePlatePlaced = false;

    void FindThePointsForPlate(SpawnWeldablesEvent spawnWeldablesEvent)
    {
        foreach (PointHolder pointHolder in pointHolders)
        {
            if (pointHolder.metalPlatePlace == spawnWeldablesEvent.metalPlaceHolder)
            {
                SpawnPerPlate(pointHolder);
            }
        }

        if (!onePlatePlaced)
        {
            EventBus<OnPlatePlacedSpaceEvent>.Publish(new OnPlatePlacedSpaceEvent());
            onePlatePlaced =true;
        }
    }

    void SpawnPerPlate(PointHolder pointHolder)
    {
        if (pointHolder.points.Count >= 2)
        {
            for (int i = 0; i < pointHolder.points.Count - 1; i++)
            {
                Vector3 currentPos = pointHolder.points[i].position;
                Vector3 direction = pointHolder.points[i + 1].position - pointHolder.points[i].position;
                direction = direction.normalized;

                currentPos += direction / dividerDistance;

                for (int j = 0; j < 50; j++) //max length to prevent a crash
                {
                    if (checkMaxWeldables(direction, currentPos, pointHolder.points[i + 1].position))
                    {
                        WeldableCube obj = Instantiate(weldableCubePrefab, currentPos, Quaternion.identity, this.transform).GetComponent<WeldableCube>();

                        obj.transform.LookAt(pointHolder.points[i + 1].position);
                        pointHolder.weldablesAmount++;
                        currentPos += direction / dividerDistance;
                    }
                }
            }

        }
        else
        {
            Debug.Log("No weldable points");
        }


        //update toatl amount of weldables
        amountOfCubes += pointHolder.weldablesAmount;
        platesSpawned++;

        EventBus<SetUpWeldableSettingsEvent>.Publish(new SetUpWeldableSettingsEvent(weldTime));
    }


}
