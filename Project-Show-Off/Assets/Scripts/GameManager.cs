using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("0 s left, 1 is right")]
    int dominantHand = 0;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    // Preferred hand methods...
    public int GetDominantHand()
    {
        return dominantHand;
    }

    public void SetDominantHand(int pDominatHand)
    {
        Debug.Log("Changed hand");
        dominantHand = pDominatHand;
    }

    // Changing scenes method...
    public void LoadSceneSpecific(int pSceneIndex)
    {
        // Check if scene to load is in bounds and then load it...
        if (pSceneIndex <= SceneManager.sceneCount && pSceneIndex >= 0)
        {
            SceneManager.LoadScene(pSceneIndex);
        }
        else { Debug.LogError("GameManager: Scene index " + pSceneIndex + " is invalid"); }
    }

    public void LoadSceneNext()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if next index is valid to load, else reset it to zero...
        if (nextSceneIndex >= SceneManager.sceneCount) { nextSceneIndex = 0; }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
