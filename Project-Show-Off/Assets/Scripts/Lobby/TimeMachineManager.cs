using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeMachineManager : MonoBehaviour
{
    [SerializeField]
    TextMeshPro yearText;
    [SerializeField]
    Renderer gameIcon;
    [SerializeField]
    LobbyButton button;

    int currentLevelIndex = 0;

    [SerializeField]
    List<Level> levels = new List<Level>();
    // Start is called before the first frame update
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
            if (levels.Count < currentLevelIndex + 2)
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

    void LoadLevelOnTimeMachine(int i)
    {
        yearText.text = levels[i].Year;
        gameIcon.material.mainTexture = levels[i].Icon.texture;
        button.SetToGoScene(levels[i].LevelIndex);
    }
}
