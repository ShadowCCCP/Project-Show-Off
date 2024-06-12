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
    public class Cat
    {
        public string[] Suspicious, Warning, Beaten;
    }

    [Serializable]
    public class Parrot
    {
        public string[] Suspicious, Warning, Beaten;
    }
    
    [Serializable]
    public class Rat
    {
        public string[] Sick, Time, List, Evening, Beautiful;
    }

    [Serializable]
    public class Alien
    {
        public string[] Suspicious, Warning, Beaten;
    }

    string CMGato = @"
    {
        'Intro' : [
            'Quick, put your preferred hand in the identifier on your right!',
            'Much better... wait, don\'t you know how dangerous this machine is?'
        ],
        'Unstable' : [
            'It\'s really unstable, but it seems you are addicted to the thrill',
            'You can press the little buttons or pull the lever to choose a time'
        ],
        'Ready' : [
            'Saxion is not responsible if anything happens to you',
            'So press the big red button on your own volition'
        ] 
    }";

    string CMPapagayo = @"
    {
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

    string CMRato = @"
    {
        'Sick' : [
            'Michelangelo called in sick, he asked you to colour the painting',
        ],
        'Time' : [
            'You only have one day though, make the best out of it!'
        ],
        'List' : [
            'The client made a list of things NOT to include, take a look' 
        ],
        'Evening' : [
            'You don\'t have much time left, put on the finishing touches'
        ],
        'Beautiful' : 
        [
            'Wow, truly remarkable.'
        ]
    }";

    string CMAlien = @"
    {
        'Broken' : [
            'The ship\'s hull has taken some damage from the meteorite rain',
            'You are equipped with magnetic boots, so that you won't fly off',
            'But watch out for incoming meteorites!'
        ]
    }";

    public Cat cat;
    public Parrot parrot;
    public Rat rat;
    public Alien alien; 

    void Awake()
    {
        cat = JsonConvert.DeserializeObject<Cat>(CMGato);
        parrot = JsonConvert.DeserializeObject<Parrot>(CMPapagayo);
        rat = JsonConvert.DeserializeObject<Rat>(CMRato);
        alien = JsonConvert.DeserializeObject<Alien>(CMAlien);
        /*
            Example how to write it down
            myTextElement.text = dialogue.LandLubber[0];
            
            or if you use my function
            ProgressText(dialogue.Landlubber (stringarray), TMP_text element (optional), line Number (optional))
        */
    }    
}
