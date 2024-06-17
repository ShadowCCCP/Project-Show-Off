using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine.UI;

public partial class DialogueManager : MonoBehaviour
{    
    public abstract class CARP
    {

    }
    public class Cat : CARP
    {
        public string[] Intro, Unstable, Ready;

    }

    public class Parrot : CARP
    {
        public string[] Suspicious, Warning, Beaten;
    }
    
    public class Rat : CARP
    {
        public string[] Sick, Time, List, Evening, Beautiful;
    }

    public class Alien : CARP
    {
        public string[] Suspicious, Warning, Beaten;
    }

    string CMGato = @"
    {
        'Intro' : [
            'Quick, put your preferred hand in the identifier on your right!',
        ],
        'Unstable' : [
            'Much better... wait, don\'t you know how dangerous this machine is?',
            'It\'s really unstable, you could end up anywhere!',
            'But if you insist, you can press the little buttons or pull the lever to choose a time'
        ],
        'Ready' : [
            'I\'ll keep an eye out for you where possible',
            'So press the big red button if you are ready for an adventure'
        ] 
    }";
    /*The player picks up the hammer and when he smashes the glass the alarm goes off
      First the alarm goes off (enable alarm lights gameobject) and after like 1 sec CMGaTo will say 'Intro'.
      After player put their hand in the hole disable the alarm gameobject and CMGaTo starts 'Unstable'
      If the lever is moved to a level CMgaTo will say 'Ready'. 
    */ 

    /*
     * on glass break -> alarm 
     * cmgato after alarm = intro (1 sec delay)
     * on first hand swtch -> alarm off and gat Unstable
     * on level switch _> ready
     * 
     */

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

    /*
        After like 1 sec when the scene starts CMPapagayo will say 'Suspicious' which will stay
        on screen untill the player picks up the sword, then CMPapagayo will say 'Warning' which 
        will stay on screen like 6(?) seconds.
        After the pirate has been beaten CMPapagayo will say 'Beaten'
    */

    /* on scene start (1 sec delay) - Suspicous 
     * on sword picup - Warning (6 sec on screem) - Warning
     * on pirate defeat  - Beaten
     */

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

    /*
        1 sec after the scene starts CMRato will say 'Sick'
        After the player picks up the pencil CMRato will say 'Time'
    */

     /* on scene start - Sick
     *  on pencil picup - Time
     *  on time almost out - Evening
     *  on paint done - Beautiful
     *  
     *  list?
     */

    string CMAlien = @"
    {
        'Broken' : [
            'The ship\'s hull has taken some damage from the meteorite rain',
            'You are equipped with magnetic boots, so that you won\'t fly off',
            'But watch out for incoming meteorites!'
        ]
    }";

    /*
     * 
     * 
     */

    public Cat cat;
    public Parrot parrot;
    public Rat rat;
    public Alien alien; 

    /*void Awake()
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
        
    }   */ 
}
