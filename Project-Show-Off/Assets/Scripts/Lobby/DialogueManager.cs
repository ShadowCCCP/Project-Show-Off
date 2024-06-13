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
    public class Dialogue
    {
        public string[] MainDialogue, BrokenGlass, Lever, Attack;
    }

    Dialogue dialogue;

    int dialogueProgress = 0;

    /*string CMPapagayo = @"{
        'MainDialogue' : [
            'hello!', 'i m saying this', 'wee woo wwee woo'
        ],
        'BrokenGlass' : [
            'Watch your left!'
        ],
        'Lever' : [
            'Right handed jonguh', 'Left handed jonguh'
        ],
        'Attack' : [
            'His guard is down. Attack!'
        ]
    }";*/

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
        dialogue = JsonConvert.DeserializeObject<Dialogue>(CMPapagayo);
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
        if (other.tag == "Controller")
        {
            startMainDialogue(dialogueProgress);
            dialogueProgress++;
        }
    }

    void startMainDialogue(int i)
    {

        if (dialogue.MainDialogue.Length > i)
        {
            textBubble.text = dialogue.MainDialogue[i];
        }
    }

    public void triggerBrokenGlassDialogue(GlassBrokenEvent glassBrokenEvent)
    {
        textBubble.text = dialogue.BrokenGlass[0];
    }
    public void triggerLeverDialogue(LeverActivatedEvent leverActivatedEvent)
    {
        textBubble.text = dialogue.Lever[0];
    }
}

