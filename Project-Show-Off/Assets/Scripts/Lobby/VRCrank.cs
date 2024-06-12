using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRCrank : MonoBehaviour
{
    [SerializeField]
    List<Collider> collidersLevels = new List<Collider>();

    [SerializeField]
    TimeMachineManager timeMachineManager;

    [SerializeField]
    Renderer knobRend;


    bool levelSelected;
    bool moveUp;
    bool moveDown;

    bool minPosReached;
    bool maxPosReached;

    bool timeMachineOn;

    Collider leverCollider;
    void Awake()
    {
        EventBus<MoveCrankEvent>.OnEvent += moveCrank;
        EventBus<GlassBrokenEvent>.OnEvent += TimeMachineOn;
    }

    void OnDestroy()
    {
        EventBus<MoveCrankEvent>.OnEvent -= moveCrank;
        EventBus<GlassBrokenEvent>.OnEvent -= TimeMachineOn;
    }

    void Start()
    {
       // knobRend.material.color = Color.red;
        timeMachineManager.LoadLevelOnTimeMachine(0);

        leverCollider = GetComponent<Collider>();
        leverCollider.enabled = false;

    }

    void Update()
    {
        preventLeverBreaking();
        updateMovementUp();
        updateMovementDown();
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < collidersLevels.Count; i++)
        {

            if (other == collidersLevels[i])
            {
                Debug.Log(i + "selected");
                levelSelected = true;
                timeMachineManager.LoadLevelOnTimeMachine(i + 1);
              //  knobRend.material.color = Color.green;

                moveUp = false;
                moveDown = false;

                if(i == collidersLevels.Count - 1)
                {
                    Debug.Log("min pos over");
                    minPosReached = true;
                }
                else if(i==0)
                {
                    Debug.Log("start pos over");
                    maxPosReached = true;
                }
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (collidersLevels.Contains(other))
        {
            levelSelected = false;
            timeMachineManager.LoadLevelOnTimeMachine(0); 
          //  knobRend.material.color = Color.red;
        }
    }

    void moveCrank(MoveCrankEvent moveCrankEvent)
    {
        if (timeMachineOn)
        {
            if (moveCrankEvent.up)
            {
                if (!maxPosReached)
                {
                    moveUp = true;
                    moveDown = false;
                    minPosReached = false;
                }
            }
            else if (!minPosReached)
            {
                moveDown = true;
                moveUp = false;
                maxPosReached = false;
            }
        }
    }

    void updateMovementDown()
    {
        if(moveUp)
        {
            transform.Translate(0.001f, 0, 0 * Time.deltaTime);
        }
    }
    void updateMovementUp()
    {
        if (moveDown)
        {
            transform.Translate(-0.001f, 0,0  * Time.deltaTime);
        }
    }

    void preventLeverBreaking()
    {
        if (transform.position.z > collidersLevels[0].transform.position.z)
        {
            transform.position = collidersLevels[0].transform.position;
        }
        else if (transform.position.z < collidersLevels[collidersLevels.Count-1].transform.position.z)
        {
            transform.position = new Vector3( collidersLevels[collidersLevels.Count-1].transform.position.x , transform.position.y, collidersLevels[collidersLevels.Count - 1].transform.position.z);
        }
    }

    void TimeMachineOn(GlassBrokenEvent glassBrokenEvent) 
    {
        timeMachineOn = true;
        leverCollider.enabled = true;
    }
}
