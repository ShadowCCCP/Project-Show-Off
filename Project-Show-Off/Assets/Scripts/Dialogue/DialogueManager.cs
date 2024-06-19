using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using static DialogueManager;

public partial class DialogueManager : MonoBehaviour
{
    //text dsspapera
    [SerializeField]
    TextMeshPro textBubble;

    [SerializeField]
    float timeBetweenText;



    void Awake()
    {   
        cat = JsonConvert.DeserializeObject<Cat>(CMGato);
        parrot = JsonConvert.DeserializeObject<Parrot>(CMPapagayo);
        rat = JsonConvert.DeserializeObject<Rat>(CMRato);
        alien = JsonConvert.DeserializeObject<Alien>(CMAlien);
        timeMachine = JsonConvert.DeserializeObject<TimeMachine>(TimeScreen);
    }

    void Start()
    {
        StartCoroutine(goThroughQueue());
    }
    
    public IEnumerator doDelayedDialogue(float time, string text)
    {
        yield return new WaitForSeconds(time);

        textBubble.text = text;
    }


    public void Speak(float time ,string text)
    {
        Debug.Log("speak: " + text);
        StartCoroutine(doDelayedDialogue(time, text));
    }

    List<string> queue = new List<string>();

    public void queueUpDialogue(string text)
    {
        queue.Add(text);
    }

    IEnumerator goThroughQueue()
    {
        if(queue.Count > 0)
        {
            yield return new WaitForSeconds(timeBetweenText);
            Speak(0,queue[0]);
            queue.RemoveAt(0);
        }

        yield return null;
        

        StartCoroutine(goThroughQueue());
    }

    public void ClearQueue()
    {
        queue.Clear();
    }

    public void SpeakRandom(float time, string[] textArrray)
    {
        string text = textArrray[Random.Range(0, textArrray.Count())];
        Debug.Log("speak: " + text);
        StartCoroutine(doDelayedDialogue(time, text));
    }
}

