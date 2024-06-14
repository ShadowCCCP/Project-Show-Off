using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueCMPapaGayo : MonoBehaviour
{
    DialogueManager dialogueManager;

    [SerializeField]
    GameObject alarm;

    string[] sceneStart;
    string[] swordPickup;
    string[] pirateDefeat;

    /* on scene start (1 sec delay) - Suspicous 
     * on sword picup - Warning (6 sec on screem) - Warning
     * on pirate defeat  - Beaten
     */
    private void Awake()
    {
        EventBus<OnSwordPickupEvent>.OnEvent += triggerSwordDialogue;
        EventBus<OnPirateDefeatedEvent>.OnEvent += triggerChangedHandDialogue;
    }

    void OnDestroy()
    {
        EventBus<OnSwordPickupEvent>.OnEvent -= triggerSwordDialogue;
        EventBus<OnPirateDefeatedEvent>.OnEvent -= triggerChangedHandDialogue;
    }


    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();


        sceneStart = dialogueManager.parrot.Suspicious;
        swordPickup = dialogueManager.parrot.Warning;
        pirateDefeat = dialogueManager.parrot.Beaten;

        speak(sceneStart, 1);

    }

    void triggerSwordDialogue(OnSwordPickupEvent onSwordPickupEvent)
    {
        speak(swordPickup, 0);
    }

    void triggerChangedHandDialogue(OnPirateDefeatedEvent onPirateDefeatedEvent)
    {
        speak(pirateDefeat, 0);
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
}
