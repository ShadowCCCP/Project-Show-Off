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
        public string[] LandLubber, Left, Right, Attack;
    }

    string CMPapagayo = @"{
        'LandLubber' : [
            'Kraaa, watch out landlubber!',
        ],
        'Left' : [
            'Watch your left!'
        ],
        'Right' : [
            'To your right!'
        ],
        'Attack' : [
            'His guard is down. Attack!'
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
