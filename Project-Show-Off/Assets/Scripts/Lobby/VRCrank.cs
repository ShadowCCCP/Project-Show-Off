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

    bool levelSelected;
    bool moveUp;
    bool moveDown;
    void Awake()
    {
        EventBus<MoveCrankEvent>.OnEvent += moveCrank;
    }

    void OnDestroy()
    {
        EventBus<MoveCrankEvent>.OnEvent -= moveCrank;
    }
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
                knobRend.material.color = Color.green;

                moveUp = false;
                moveDown = false;
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (collidersLevels.Contains(other))
        {
            levelSelected = false;
            timeMachineManager.LoadLevelOnTimeMachine(0); 
            knobRend.material.color = Color.red;
        }
    }

    void moveCrank(MoveCrankEvent moveCrankEvent)
    {
        if (moveCrankEvent.up )
        {
            moveUp = true;
        }
        else
        {
            moveDown = true;
        }
    }

    void updateMovementDown()
    {
        if((!levelSelected || transform.position.y >= minPos.position.y) && moveUp)
        {
             transform.Translate(0, 0, 0.1f * Time.deltaTime);
        }
    }
    void updateMovementUp()
    {
        if ((!levelSelected || transform.position.y <= stratPos.y )&& moveDown)
        {
            transform.Translate(0, 0, -0.1f * Time.deltaTime);
        }
    }
}
