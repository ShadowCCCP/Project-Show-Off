using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DialogueManager;

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



    void Awake()
    {   
        cat = JsonConvert.DeserializeObject<Cat>(CMGato);
        parrot = JsonConvert.DeserializeObject<Parrot>(CMPapagayo);
        rat = JsonConvert.DeserializeObject<Rat>(CMRato);
        alien = JsonConvert.DeserializeObject<Alien>(CMAlien);
    }

    void Start()
    {
        StartCoroutine(goThroughQueue());
    }

    void Update()
    {


    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTimed)
        {
            if (other.tag == "RightController" || other.tag == "LeftController")
            {
                //startMainDialogue(dialogueProgress);
                //dialogueProgress++;
            }
        }
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
}

