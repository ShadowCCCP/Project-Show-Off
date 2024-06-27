using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;


/// <summary>
/// Handles the time machine screen and some other functionality
/// Can load levels
/// Can turn the machine on and off
/// </summary>
public class TimeMachineManager : MonoBehaviour
{
    [SerializeField]
    TextMeshPro yearText;
    [SerializeField]
    TextMeshPro dangerText;
    [SerializeField]
    TextMeshPro handText;
    [SerializeField]
    SpriteRenderer gameIcon;
    [SerializeField]
    LobbyButton button;
    [SerializeField]
    TextMeshPro bigText;


    [SerializeField]
    List<Level> levels = new List<Level>();

    [SerializeField]
    bool On = false;

    [SerializeField]
    Renderer wires;

    [SerializeField]
    Material litWireMat;

    void Awake()
    {
        EventBus<ChangeHandEvent>.OnEvent += OnHandChange;
    }

    void OnDestroy()
    {
        EventBus<ChangeHandEvent>.OnEvent -= OnHandChange;
    }
    void Start()
    {
        if (levels.Count > 0)
        {
            LoadLevelOnTimeMachine(4);
        }
        else
        {
            Debug.Log("no levels assigned");
        }
        if(On)
        {
             turnOn();
             LoadLevelOnTimeMachine(0);
        } 
        
    }

    public void LoadLevelOnTimeMachine(int i)
    {
        //year, danger and hand
        emptytextCheck(levels[i].Year, "Year: ", yearText);
        emptytextCheck(levels[i].Danger, "Danger: ", dangerText);
        if (levels[i].Year != "" && GameManager.Instance) { emptytextCheck(GameManager.Instance.GetDominantHandString(), "Dominant hand: ", handText); }
        else { handText.text = ""; }

        //icon
        if (levels[i].Icon != null)
        {
            gameIcon.gameObject.SetActive(true);
            gameIcon.sprite = levels[i].Icon;


            //on level selecet to updated dialogue 
            // only if an icon is present the level is an actual level
            EventBus<OnLevelSelectedOnTMEvent>.Publish(new OnLevelSelectedOnTMEvent());
        }
        else
        {
            gameIcon.gameObject.SetActive(false);
        }

        //big bacground text
        emptytextCheck(levels[i].BigText, "", bigText);


        //button
        button.SetToGoScene(levels[i].LevelIndex);

        if (levels[i].LevelIndex >= 0)
        {
            button.SetMaterialLit();
        }
        else
        {
            button.SetMaterialUnLit();
        }
    }

    void emptytextCheck(string textToWrite, string textYearOrDanger, TextMeshPro textMesh)
    {
        if (textToWrite != "")
        {
            textMesh.text = textYearOrDanger + textToWrite;
        }
        else
        {
            textMesh.text = " ";
        }
    }

    public void LoadTextOnTimeMachine(string text)
    {
        yearText.text = "";
        dangerText.text = "";
        handText.text = "";
        gameIcon.gameObject.SetActive(false);
        button.SetToGoScene(-1);

        emptytextCheck(text, "", bigText);

    }

    public bool GetTimeMachineState()
    {
        return On;
        
    }

    void OnHandChange(ChangeHandEvent changeHandEvent)
    {
        if (!On)
        {
          turnOn();
        }

    }

    void turnOn(){
           On = true;
            LoadLevelOnTimeMachine(0);
            wires.material = litWireMat;
    }
    
}
