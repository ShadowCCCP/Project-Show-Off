using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using TMPro;
using UnityEngine.UI;

public partial class DialogueManager : MonoBehaviour
{    
    public class Cat
    {
        public string[] Intro, Gap, Select, Ready;

    }

    public class Parrot 
    {
        public string[] Suspicious, Warning, Beaten;
    }
    
    public class Rat 
    {
        public string[] Sick, Time, List, Evening, Beautiful;
    }

    public class Alien 
    {
        public string[] Destroyed, Meteor, Shoes, Trigger;
    }

    public class TimeMachine 
    {
        public string[] Broken, Smash, Identify, Select;
    }

    public class TimeGod
    {
        public string[] Anomaly, Succeeded, Failed;
    }

    string CMGato = @"
    {
        'Intro' : [
            'Grab the hammer with the controller button at your middle finger and break the glass to start'
        ],
        'Gap' : [
            'Glad they finally sent someone, can you fully put your arm in the blue gap'
        ],
        'Select' : [
            'Use the lever or press the buttons to select a timeline to fix',
        ],
        'Ready' : [
            'If you ever feel distressed, press the button at your feet!',
            'I\'ll help you where I can, take care timeline repair person.'
        ] 
    }";

    string TimeScreen = @"{
        'Broken' : [
            'Broken timelines detected'
        ],
        'Smash' : [
            'Smash glass to override'
        ],
        Identify : [
            'Identify yourself'
        ],
        'Select' : [
            'Select broken timeline'
        ]

    }";

    //string CMInbetween = @


    //PC says: BROKEN TIMELINES DETECTED
    //CMGaTo and Hammer comes spinning down
    //CMGaTo says after animation: Grab the hammer with the controller button at your middle finger and break the glass!
    
    //When player picks up hammer
    //PC says: SMASH GLASS TO OVERRIDE
    
    //When glass has been broken
    //Alarm goes off
    //PC says: IDENTIFY YOURSELF
    //Blue material lights up and disable (hand spotlight)
    //Glad they finally sent someone, can you fully put your arm and the hammer in the blue gap?
    
    
    //PC says: SELECT BROKEN TIMELINE
    //Alarm stops
    //CMGato says: Use the lever or press the buttons to select a timeline to fix
    //The lever and buttons light up
    
    //When a timeline has been chosen
    //CMGato says ready: I'll help you where I can, take care timeline repair person.  
    //Red button lights up

    //Timeline Repair Simulator 

    
    
    
    
    
    
    
    
    
    
    
    /*The player picks up the hammer and when he smashes the glass the alarm goes off
      First the alarm goes off (enable alarm lights gameobject) and after like 1 sec CMGaTo will say 'Intro'.
      After player put their hand in the hole disable the alarm gameobject and CMGaTo starts 'Unstable'
      If the lever is moved to a level CMgaTo will say 'Ready'. 
    */ 

    //

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
            'Wow, truly remarkable.',
            'Is it like... modern art I guess?',
            'You truly put your personal touch to it',
            'Do you think what you drew was appropiate?',
            'I\'ve never seen something so beautiful'
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
     *  list? - andreas did it
     */

    //pic random beautiful

    string CMAlien = @"
    {
        'Destroyed' : [
            'A meteor has hit our ship! Can you get the loose plates and reweld the broken parts?'
        ], 
        'Meteor' : [
            'Watch out for meteors!' 
        ],
        'Shoes' : [
            'Get the plates that got loose, they are still drifting around'
        ],
        'Trigger' : [
            'Use the trigger at your index finger to activate the blowtorch'
        ]
    }";

     /*
     *  dark room + cmet with u + cmet : destroyed
     * 
     * presss button -> door opens -> meteor fles by cmet : meteor -> plates snapped => cmet: trigger 
     */

    string CMTimegod = @"
    {
        'Anomaly' : [
            'A temporal anomaly appeared, but not to worry, I spawned this portal to bring you back'
        ],

        'Succeeded' : [
            'You did great back there, you truly excel as a timeline repair person'
        ],

        'Failed' : [
            'Don\'t worry, I can give you another chance timeline repair person'
        ]
    
    }";


    /* 
     * anomaly always start 5 sec
     * then based on game (succeeded / failed)
     * 
     */

    public Cat cat;
    public Parrot parrot;
    public Rat rat;
    public Alien alien; 
    public TimeMachine timeMachine;
    public TimeGod timegod;

}
