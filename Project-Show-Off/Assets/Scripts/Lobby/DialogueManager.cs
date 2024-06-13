using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogueManager;
//using static JSON_Text;

public partial class DialogueManager : MonoBehaviour
{
    //text dsspapera
    [SerializeField]
    TextMeshPro textBubble;

    [Tooltip("If true touching the animal no longer works")]
    [SerializeField]
    bool isTimed;

    [SerializeField]
    float timeBetweenText;

    enum CompanionCMGT { CMGato, CMPapagayo, CMRato, CMAlien }

    [SerializeField]
    CompanionCMGT companion;

    int dialogueProgress = 0;


    void Awake()
    {   
        cat = JsonConvert.DeserializeObject<Cat>(CMGato);
        parrot = JsonConvert.DeserializeObject<Parrot>(CMPapagayo);
        rat = JsonConvert.DeserializeObject<Rat>(CMRato);
        alien = JsonConvert.DeserializeObject<Alien>(CMAlien);
        
        EventBus<LeverActivatedEvent>.OnEvent += triggerLeverDialogue;
        EventBus<GlassBrokenEvent>.OnEvent += triggerBrokenGlassDialogue;
    }

    void OnDestroy()
    {
        EventBus<LeverActivatedEvent>.OnEvent -= triggerLeverDialogue;
        EventBus<GlassBrokenEvent>.OnEvent -= triggerBrokenGlassDialogue;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isTimed)
        {
            StartCoroutine(timedDialogue());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            startMainDialogue(dialogueProgress);
            dialogueProgress++;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTimed)
        {
            if (other.tag == "RightController" || other.tag == "LeftController")
            {
                startMainDialogue(dialogueProgress);
                dialogueProgress++;
            }
        }
    }

    void startMainDialogue(int i)
    {

       //// if (dialogue.MainDialogue.Length > i)
       // {
            //textBubble.text = dialogue.MainDialogue[i];
       // }
    }

    void triggerBrokenGlassDialogue(GlassBrokenEvent glassBrokenEvent)
    {
       // textBubble.text = dialogue.BrokenGlass[0];
    }
    void triggerLeverDialogue(LeverActivatedEvent leverActivatedEvent)
    {
        //textBubble.text = dialogue.Lever[0];
    }

    
    IEnumerator timedDialogue()
    {
       // if (dialogue.MainDialogue.Length > dialogueProgress)
       // {
            startMainDialogue(dialogueProgress);
            dialogueProgress++;
            yield return new WaitForSeconds(timeBetweenText);
            StartCoroutine(timedDialogue());
       // }
    }
}

