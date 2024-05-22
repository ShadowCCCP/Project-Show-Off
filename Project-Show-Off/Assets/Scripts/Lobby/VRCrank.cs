using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VRCrank : MonoBehaviour
{
    [SerializeField]
    List<Collider> collidersLevels = new List<Collider>();

    [SerializeField]
    TimeMachineManager timeMachineManager;

    [SerializeField]
    Transform minPos;

    [SerializeField]
    Renderer knobRend;

    Vector3 stratPos;
    void Start()
    {
        stratPos = transform.position;
        knobRend.material.color = Color.red;

    }

    void Update()
    {
        if (transform.position.y > stratPos.y)
        {
            transform.position = stratPos;
        }
        else if (transform.position.y <  minPos.position.y)
        {
            transform.position = new Vector3(0, minPos.position.y,0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < collidersLevels.Count; i++)
        {

            if (other == collidersLevels[i])
            {
                Debug.Log(i + "selected");
                timeMachineManager.LoadLevelOnTimeMachine(i + 1);
                knobRend.material.color = Color.green;
            }
        }




    }

    private void OnTriggerExit(Collider other)
    {
        if (collidersLevels.Contains(other))
        {
            timeMachineManager.LoadLevelOnTimeMachine(0); 
            knobRend.material.color = Color.red;
        }
    }
}
