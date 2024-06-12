using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeMachineManager : MonoBehaviour
{
    [SerializeField]
    TextMeshPro yearText;
    [SerializeField]
    TextMeshPro dangerText;
    [SerializeField]
    Renderer gameIcon;
    [SerializeField]
    LobbyButton button;
    [SerializeField]
    TextMeshPro bigText;

    int currentLevelIndex = 0;

    [SerializeField]
    List<Level> levels = new List<Level>();
    void Start()
    {
        if (levels.Count > 0)
        {
            LoadLevelOnTimeMachine(0);
        }
        else
        {
            Debug.Log("no levels assigned");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (levels.Count > currentLevelIndex +1)
            {
                currentLevelIndex++;
            }
            else
            {
                currentLevelIndex--;
            }
            LoadLevelOnTimeMachine(currentLevelIndex);
        }
    }

    public void LoadLevelOnTimeMachine(int i)
    {
        //year and danger
        emptytextCheck(levels[i].Year, "Year: ");
        emptytextCheck(levels[i].Danger, "Danger: ");

        //icon
        if (levels[i].Icon.texture != null)
        {
            gameIcon.gameObject.SetActive(true);
            gameIcon.material.mainTexture = levels[i].Icon.texture;
        }
        else
        {
            gameIcon.gameObject.SetActive(false);
        }

        //button
        button.SetToGoScene(levels[i].LevelIndex);
        //
        bigText.SetText(levels[i].BigText);
    }

    void emptytextCheck(string text, string textYearOrDanger)
    {
        if (text != "")
        {
            yearText.text = textYearOrDanger + text;
        }
        else
        {
            yearText.text = " ";
        }
    }

    
}
