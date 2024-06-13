using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueCMGato : MonoBehaviour
{
    DialogueManager dialogueManager;

    [SerializeField]
    GameObject alarm;

    string[] brokenGlass;
    string[] handSelect;
    string[] levelSwitch;


    /*
    * on glass break -> alarm 
    * cmgato after alarm = intro (1 sec delay)
    * on first hand swtch -> alarm off and cat Unstable
    * on level switch -> ready
    * 
    */
    private void Awake()
    {
        EventBus<GlassBrokenEvent>.OnEvent += triggerBrokenGlassDialogue;
        EventBus<ChangeHandEvent>.OnEvent += triggerChangedHandDialogue;
        EventBus<OnLevelSelectedOnTMEvent>.OnEvent += triggerChangedLevelDialogue;
    }

    void OnDestroy()
    {
        EventBus<GlassBrokenEvent>.OnEvent -= triggerBrokenGlassDialogue;
        EventBus<ChangeHandEvent>.OnEvent -= triggerChangedHandDialogue;
        EventBus<OnLevelSelectedOnTMEvent>.OnEvent -= triggerChangedLevelDialogue;
    }


    void Start()
    {
        dialogueManager = GetComponent<DialogueManager>();


        brokenGlass = dialogueManager.cat.Intro;
        handSelect = dialogueManager.cat.Unstable;
        levelSwitch = dialogueManager.cat.Ready;

    }
   
    void triggerBrokenGlassDialogue(GlassBrokenEvent glassBrokenEvent)
    {
        //start alarm
        alarm.SetActive(true);

        speak(brokenGlass,1);
    }

    void triggerChangedHandDialogue(ChangeHandEvent changeHandEvent)
    {
        //alarm off
        alarm.SetActive(false);

        speak(handSelect,0);
    }
    void triggerChangedLevelDialogue(OnLevelSelectedOnTMEvent onLevelSelectedOnTMEvent)
    {
        speak(levelSwitch,0);
    }

    void speak(string[] text , float time)
    {
        dialogueManager.ClearQueue();

        dialogueManager.Speak(time, text[0]);

        if(text.Count() > 1)
        {
            for (int i = 1; i< text.Count(); i++)
            {
                dialogueManager.queueUpDialogue(text[i]);
            }
        }
    }

}
