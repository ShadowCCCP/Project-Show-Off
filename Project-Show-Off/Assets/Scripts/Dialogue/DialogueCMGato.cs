using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DialogueCMGato : MonoBehaviour
{
    DialogueManager dialogueManager;

    [SerializeField]
    TimeMachineManager timeMachineManager;

    [SerializeField]
    GameObject handSpotlight;


    string[] intro;
    string[] gap;
    string[] selectCat;
    string[] ready;

    string[] smash;
    string[] identify;
    string[] selectTM;
    /*
    * on glass break -> alarm 
    * cmgato after alarm = intro (1 sec delay)
    * on first hand swtch -> alarm off and cat Unstable
    * on level switch -> ready
    * 
    */

    bool canDoIdentify = false; 
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


        intro = dialogueManager.cat.Intro;
        gap = dialogueManager.cat.Gap;
        selectCat = dialogueManager.cat.Select;
        ready = dialogueManager.cat.Ready;

        smash = dialogueManager.timeMachine.Smash;
        identify = dialogueManager.timeMachine.Identify;
        selectTM = dialogueManager.timeMachine.Select;

    }
   
    void triggerBrokenGlassDialogue(GlassBrokenEvent glassBrokenEvent)
    {
        if (canDoIdentify)
        {
            identfy();
        }
        else
        {
            StartCoroutine(waitForIdentify());
        }
    }

    IEnumerator waitForIdentify()
    {
        yield return new WaitForSeconds(2);
        identfy();
    }

    void identfy()
    {
        speak(gap, 0);
        speakOnScreen(identify[0]);
    }

    void triggerChangedHandDialogue(ChangeHandEvent changeHandEvent)
    {

        speakOnScreen(selectTM[0]);
        speak(selectCat, 0);

        //lever and buttons lih tup

    }
    void triggerChangedLevelDialogue(OnLevelSelectedOnTMEvent onLevelSelectedOnTMEvent)
    {
        speak(ready,0);

        //red button
    }

    public void OnAnimationEnd()
    {
        speak(intro, 0);
        speakOnScreen(smash[0]);
        canDoIdentify = true;
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

    void speakOnScreen(string text)
    {
        timeMachineManager.LoadTextOnTimeMachine(text);
    }



}
