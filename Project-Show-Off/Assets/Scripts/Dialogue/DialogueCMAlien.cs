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

    [SerializeField]
    Transform buble;

    [SerializeField]
    Transform startPos;

    [SerializeField]
    Transform bubleStartPos;

    [SerializeField]
    Animator meteorAnim;

    Transform normalPos;

    Transform bubleNormalPos;

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

        alienTeleportSetup();

    }
    void triggerDoorOpenDialogue(OnDoorOpenSpaceEvent onDoorOpenSpaceEvent)
    {
        speak(meteor, 2);

        //do meteor animator meteorAnim.setTrigger("");

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

        alienTeleport();
    }

    void alienTeleport()
    {
        transform.position = normalPos.position;
        transform.rotation = normalPos.rotation;

        buble.position = bubleNormalPos.position;
        buble.rotation = bubleNormalPos.rotation;
    }

    void alienTeleportSetup()
    {
        normalPos = transform;
        bubleNormalPos = buble.transform;

        //go to the start position
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;

        buble.position = bubleStartPos.position;
        buble.rotation = bubleStartPos.rotation;
    }
}
