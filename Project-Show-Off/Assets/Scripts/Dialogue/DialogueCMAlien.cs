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

    Vector3 normalPos;

    Quaternion normalRot;

    Vector3 bubleNormalPos;

    Quaternion bubleNormalRot;

    string[] destroyed;
    string[] meteor;
    string[] shoes;
    string[] trigger;

    [SerializeField]
    float timeForMeteor = 4;

    [SerializeField]
    Animation meteorFall;

    [SerializeField]
    MeteoriteManager meteoriteManager;

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
        alienTeleportSetup();

        destroyed = dialogueManager.alien.Destroyed;
        meteor = dialogueManager.alien.Meteor;
        shoes = dialogueManager.alien.Shoes;
        trigger = dialogueManager.alien.Trigger;

        speak(destroyed, 1);

        

    }
    void triggerDoorOpenDialogue(OnDoorOpenSpaceEvent onDoorOpenSpaceEvent)
    {
        speak(meteor, 2);

        meteorFall.enabled=true;

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
        speak(shoes, 0);
        meteoriteManager.StartSpawning();
    }

    void alienTeleport()
    {

        Debug.Log("attempt");
        transform.position = normalPos;
        transform.rotation = normalRot;

        buble.position = bubleNormalPos;
        buble.rotation = bubleNormalRot;


        //dialogue for outside
    }

    void alienTeleportSetup()
    {
        normalPos = transform.position;
        normalRot = transform.rotation;

        bubleNormalPos = buble.transform.position;
        bubleNormalRot = buble.transform.rotation;

        //go to the start position
        transform.position = startPos.position;
        transform.rotation = startPos.rotation;

        buble.position = bubleStartPos.position;
        buble.rotation = bubleStartPos.rotation;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            alienTeleport();
        }
    }
}
