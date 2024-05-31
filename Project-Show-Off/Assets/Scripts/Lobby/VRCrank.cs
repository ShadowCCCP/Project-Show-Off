using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

        if (Input.GetKey(KeyCode.J))
        {
            moveUp = true;
        }
        if (Input.GetKey(KeyCode.N))
        {
            moveDown = true;
        }

        
        if (transform.position.z > stratPos.z)
        {
            Debug.Log(transform.position.z + " " + stratPos.z   );
            Debug.Log("start pos over");
            transform.position = stratPos;
            moveUp = false;
            moveDown = false;
        } 
        else if (transform.position.z <  minPos.position.z)
        {
            Debug.Log("min pos over");
            transform.position = new Vector3(stratPos.z, stratPos.y, minPos.position.z);
            moveUp = false;
            moveDown = false;
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
            moveDown = false;
        }
        else
        {
            moveDown = true;
            moveUp = false;
        }
    }

    void updateMovementDown()
    {
        if((!levelSelected || transform.position.z >= minPos.position.z) && moveUp)
        {
            transform.Translate(0.01f, 0, 0 * Time.deltaTime);
        }
    }
    void updateMovementUp()
    {
        if ((!levelSelected || transform.position.z <= stratPos.z )&& moveDown)
        {
            transform.Translate(-0.01f, 0,0  * Time.deltaTime);
        }
    }
}
