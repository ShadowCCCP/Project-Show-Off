using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRCrank : MonoBehaviour
{
    [SerializeField]
    List<Collider> collidersLevels = new List<Collider>();

    [SerializeField]
    float leverSpeed = 2;

    [SerializeField]
    TimeMachineManager timeMachineManager;


    bool levelSelected;
    bool moveUp;
    bool moveDown;

    bool minPosReached;
    bool maxPosReached;

    bool timeMachineOn;

    Collider leverCollider;

    [SerializeField]
    SoundPlayer soundPlayer;
    [SerializeField]
    SoundPlayer soundPlayerLooped;
    Vector3 oldPos;
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
        timeMachineManager.LoadLevelOnTimeMachine(0);

        leverCollider = GetComponent<Collider>();

        leverCollider.enabled = false;

        oldPos = transform.position;

    }

    void Update()
    {
        preventLeverBreaking();
        updateMovementUp();
        updateMovementDown();
        soundsForLever();
    }

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < collidersLevels.Count; i++)
        {

            if (other == collidersLevels[i])
            {
                Debug.Log(i + "selected");
                levelSelected = true;
                StartCoroutine(slowLevelLoad(i + 1));


                moveUp = false;
                moveDown = false;

                if(i == collidersLevels.Count - 1)
                {
                    Debug.Log("min pos over");
                    minPosReached = true;
                    maxPosReached = false;
                }
                else if(i==0)
                {
                    Debug.Log("start pos over");
                    maxPosReached = true;
                    minPosReached = false;
                }
            }
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (collidersLevels.Contains(other))
        {
            levelSelected = false;
            StartCoroutine(slowLevelLoad(0));
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
            transform.Translate(0.001f, 0, 0 * Time.deltaTime * leverSpeed);
        }
    }
    void updateMovementUp()
    {
        if (moveDown)
        {
            transform.Translate(-0.001f, 0,0  * Time.deltaTime * leverSpeed);
        }
    }

    void preventLeverBreaking()
    {
        if (transform.position.z > collidersLevels[0].transform.position.z)
        {
            transform.position = new Vector3(collidersLevels[0].transform.position.x, transform.position.y, collidersLevels[0].transform.position.z) ;
            maxPosReached = true;
            minPosReached=false;
        }
        else if (transform.position.z < collidersLevels[collidersLevels.Count-1].transform.position.z )
        {
            transform.position = new Vector3( collidersLevels[collidersLevels.Count-1].transform.position.x , transform.position.y, collidersLevels[collidersLevels.Count - 1].transform.position.z);
            minPosReached = true;
            maxPosReached=false;
        }
    }

    void TimeMachineOn(GlassBrokenEvent glassBrokenEvent) 
    {
        timeMachineOn = true;
        leverCollider.enabled = true;
    }

    IEnumerator slowLevelLoad(int levelIndex)
    {
        yield return new WaitForSeconds(0.2f);
        if (levelSelected)
        {
            timeMachineManager.LoadLevelOnTimeMachine(levelIndex);
        }
        else
        {
            timeMachineManager.LoadLevelOnTimeMachine(0);
        }
    }



    bool moving;

    void soundsForLever()
    {
        if(transform.position != oldPos)
        {
            if (!moving)
            {
                soundPlayer.PlaySpecific(0);
                moving = true; 
                soundPlayerLooped.Play();
            }
        }
        else if (moving)
        {
            soundPlayerLooped.Stop();
            soundPlayer.PlaySpecific(1);
            moving = false;
        }



        oldPos = transform.position;
    }
}
