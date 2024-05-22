using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TextHandler : MonoBehaviour
{

    //Delegates to invoke functions before, during or after the line or dialogue.
    //We probably dont need this?
    internal delegate void TextEvents();
    internal TextEvents afterDialogue;
    internal TextEvents startDialogue;
    internal TextEvents duringLine;

    [SerializeField] TMP_Text showText;
    
    [SerializeField] TMP_Text[] textUI;
    bool skippable = true;

    /*
        Hey hey, I already removed some stuff, but there still might be some bloat, since I made it so that 
        you can switch between multiple ui text elements, but we wont need that. 
        I hope it still works :')
    */

    /* 
        I see that I didnt do a check whether the line you can pass through as a parameter is out of bounds
        Dont see a reason to not start at the first line, but you never know
    */

    void Awake()
    {
        textUI = new TMP_Text[] {showText};
    }

    public void ProgressText(string[] stringArray, TMP_Text text = null, int line = 0)
    {     
        skippable = true; 
        
        //unless specified, text will default to a TMP_Text value you set (the first element in the textUI array)
        text = text ?? textUI[0];

        //Invoke event before dialogue
        startDialogue?.Invoke();
        startDialogue = null;
        
        StartCoroutine(Manual(stringArray, text, line));                
    }

    bool talking;
    object[] myObjects;

    IEnumerator Manual(string[] stringArray, TMP_Text myText, int line, float pause = 0)
    {
        if(line != stringArray.Length )
        {
            myText.text = "";
            //Check for metadata to invoke different functions
            DynamicConversation(stringArray, ref myText, line, ref pause);
            
            yield return new WaitForSeconds(pause);
            
            //set ui text to the string
            myText.text = stringArray[line];
        }

        if(line < stringArray.Length)
        {
            //set the next line in the object array to invoke when you continue to the next line in update
            line++; 
            //I never set talking to false during a conversation, 
            //yet when I remove these two lines it simply doesn't work for some reason :)
            yield return new WaitForEndOfFrame();
            talking = true;
        } 

        else if(line == stringArray.Length)
        {
            myText.text = "";

            talking = false;

            afterDialogue?.Invoke();
            afterDialogue = null;
        }

        myObjects = new object[] {stringArray, myText, line};
    }

    void DynamicConversation(string[] stringArray, ref TMP_Text myText, int line, ref float pause)
    {
        //Check for 'metadata'
        if(!stringArray[line].Contains("#"))
        {
            return;
        }

        //Split off the main string
        string[] meta = stringArray[line].Split("#");
        var finalString = meta[0];

        //Functions what to do depending on the metadata
        if(meta.Length > 1)
        {
            for(int i = 1; i < meta.Length; i++)
            {
                switch(meta[i])
                {
                    case "noskip":
                        skippable = false;
                        break;
                    case "event":
                        duringLine?.Invoke();
                        duringLine = null;
                        break;
                    case string s when s.Contains("wait"):
                        string[] getWaitTime = meta[i].Split("=");
                        var wait = float.Parse(getWaitTime[1]);
                        pause = wait;
                        break;
                }    
            }        
        }
        
        //set the concurrent line through a reference parameter
        stringArray[line] = finalString;
    }

    void Update()
    {
        if(talking == true)
        {
            if(Input.GetMouseButtonDown(0) && skippable == true)
            {
                //Convert ObjectArray
                var myArr = myObjects[0] as string[];
                var myTMP = myObjects[1] as TMP_Text;
                var myNumber = (int)myObjects[2];

                //Show next string on screen
                StartCoroutine(Manual(myArr, myTMP, myNumber));
            }
        }
    }

}
