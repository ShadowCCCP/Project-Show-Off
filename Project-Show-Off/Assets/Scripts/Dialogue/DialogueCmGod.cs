using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueCmGod : MonoBehaviour
{
    DialogueManager dialogueManager;

    string[] anomaly;
    string[] succeeded;
    string[] failed;

    bool success;

    /* 
   * anomaly always start 5 sec
   * then based on game (succeeded / failed)
   * 
   */
    private void Awake()
    {
        //EventBus<GlassBrokenEvent>.OnEvent += triggerBrokenGlassDialogue;
    }

    void OnDestroy()
    {
        //EventBus<GlassBrokenEvent>.OnEvent -= triggerBrokenGlassDialogue;
    }


    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();

        if (GameManager.Instance !=null)
        {
            success = GameManager.Instance.wonState;
        }
        else
        {
            Debug.Log("Missing Game Manager");
        }

        anomaly = dialogueManager.timegod.Anomaly;
        succeeded = dialogueManager.timegod.Succeeded;
        failed = dialogueManager.timegod.Failed;

        speak(anomaly, 0);

        if (success)
        {
            speak(succeeded, dialogueManager.GetTimeBetween());
        }
        else
        {
            speak(failed, dialogueManager.GetTimeBetween());
        }

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
