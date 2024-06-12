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
        emptytextCheck(levels[i].Year, "Year: ", yearText);
        emptytextCheck(levels[i].Danger, "Danger: ", dangerText);

        //icon
        if (levels[i].Icon != null)
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
        emptytextCheck(levels[i].BigText, "", bigText);
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

    
}
