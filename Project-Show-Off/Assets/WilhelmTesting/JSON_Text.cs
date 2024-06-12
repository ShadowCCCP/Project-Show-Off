using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine.UI;

public class JSON_Text : MonoBehaviour
{    
    [Serializable]
    public class Dialogue
    {
        public string[] Suspicious, Warning, Beaten;
    }

    string CMGato = @"
    {
        'Intro' : [
            'Quick, put your preferred hand in identifier on your right!',
            'Much better... wait, weren't you banned from using the timemachine?'
        ],
        'Unstable' : [
            'It is really unstable, but it seems you are addicted to the thrill',
            'You can press the buttons or pull the lever to choose a time, as you know'
        ],
        'Ready' : [
            'Saxion is not responsible if anything happens to you',
            'So press the big red button on your own risk'
        ] 
    }";

    string CMPapagayo = @"{
        'Suspicious' : [
            'What\'s your sword doing at the end of the plank, captain?' 
        ],
        'Warning' : [
            
            'Watch out, it\'s a mutiny!'
        ],
        'Beaten' : [ 
            'Good job! He stood no chance against your swordmanship!'
        ]
    }";

    string CMRato = @"{
        'Sick' : [
            'Michelangelo called in sick, he asked you to colour the painting',
            'You only have one day though, make the best out of it!'
        ],
        'List' : [
            'The client made a list of things NOT to include, take a look' 
        ],
        'Evening' : [
            'You don\'t have much time left, put on the finishing touches'
        ]
    }";

    public Dialogue dialogue; 

    void Awake()
    {
        dialogue = JsonConvert.DeserializeObject<Dialogue>(CMPapagayo);
        /*
            Example how to write it down
            myTextElement.text = dialogue.LandLubber[0];
            
            or if you use my function
            ProgressText(dialogue.Landlubber (stringarray), TMP_text element (optional), line Number (optional))
        */
    }    
}
