using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueCMAlien : MonoBehaviour
{
    [SerializeField]
    GameObject mainCamera;
    DialogueManager dialogueManager;

    string[] sceneStart;
    string[] pencilPickup;
    string[] timeRunningOut;
    string[] paintDone;

    /* on scene start - Sick
  *  on pencil picup - Time
  *  on time almost out - Evening
  *  on paint done - Beautiful
  *  
  *  list?
  */
    private void Awake()
    {
        EventBus<OnPencilPickupEvent>.OnEvent += triggerPencilDialogue;
        EventBus<PaintTimeRunningOutEvent>.OnEvent += triggerTimeRunningOutDialogue;
        EventBus<PaintDoneEvent>.OnEvent += triggerpaintDoneDialogue;
    }

    void OnDestroy()
    {
        EventBus<OnPencilPickupEvent>.OnEvent -= triggerPencilDialogue;
        EventBus<PaintTimeRunningOutEvent>.OnEvent -= triggerTimeRunningOutDialogue;
        EventBus<PaintDoneEvent>.OnEvent -= triggerpaintDoneDialogue;
    }


    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();


        sceneStart = dialogueManager.rat.Sick;
        pencilPickup = dialogueManager.rat.Time;
        timeRunningOut = dialogueManager.rat.Evening;
        paintDone = dialogueManager.rat.Beautiful;

        speak(sceneStart, 0);

    }

    void triggerPencilDialogue(OnPencilPickupEvent onPencil)
    {
        speak(pencilPickup, 0);
    }

    void triggerTimeRunningOutDialogue(PaintTimeRunningOutEvent paintTimeRunningOutEvent)
    {
        speak(timeRunningOut, 0);
    }
    void triggerpaintDoneDialogue(PaintDoneEvent paintDoneEvent)
    {
        speak(paintDone, 0);
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
