using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueCMAlien : MonoBehaviour
{
    [SerializeField]
    GameObject mainCamera;
    DialogueManager dialogueManager;

    string[] destroyed;
    string[] meteor;
    string[] trigger;

    [SerializeField]
    float timeForMeteor = 4;

    /*
    *  dark room + cmet with u + cmet : destroyed
    * 
    * presss button -> door opens -> meteor fles by cmet : meteor -> plates snapped => cmet: trigger 
    */
    private void Awake()
    {
        EventBus<OnDoorOpenSpaceEvent>.OnEvent += triggerDoorOpenDialogue;
        EventBus<OnPlatePlacedSpaceEvent>.OnEvent += triggerPlateDialogue;
    }

    void OnDestroy()
    {
        EventBus<OnDoorOpenSpaceEvent>.OnEvent -= triggerDoorOpenDialogue;
        EventBus<OnPlatePlacedSpaceEvent>.OnEvent -= triggerPlateDialogue;
    }


    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();


        destroyed = dialogueManager.alien.Destroyed;
        meteor = dialogueManager.alien.Meteor;
        trigger = dialogueManager.alien.Trigger;

        speak(destroyed, 1);
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            EventBus<GoBackToStartPosEvent>.Publish(new GoBackToStartPosEvent());
        }
    }
    void triggerDoorOpenDialogue(OnDoorOpenSpaceEvent onDoorOpenSpaceEvent)
    {
        speak(meteor, 0);

        StartCoroutine(gobackToOriginDelayed());
    }

    void triggerPlateDialogue(OnPlatePlacedSpaceEvent onPlatePlacedSpaceEvent)
    {
        speak(trigger, 0);
    }

    void speak(string[] text, float time)
    {
        dialogueManager.ClearQueue();

        dialogueManager.Speak(time, text[0]);

        if (text.Count() > 1)
        {
            for (int i = 1; i < text.Count(); i++)
            {
                dialogueManager.queueUpDialogue(text[i]);
            }
        }
    }

    IEnumerator gobackToOriginDelayed()
    {
        yield return new WaitForSeconds(timeForMeteor);
        EventBus<GoBackToStartPosEvent>.Publish(new GoBackToStartPosEvent());
    }
}
