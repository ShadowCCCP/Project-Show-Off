using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //transitions here
    public static GameManager Instance { get; private set; }

    [SerializeField]
    [Tooltip("0 s left, 1 is right")]
    int dominantHand = 0;

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
        StartCoroutine(LoadSceneSpecificRoutine(pSceneIndex));
    }

    public void LoadSceneNext()
    {
        StartCoroutine(LoadSceneNextRoutine() );
    }

    IEnumerator LoadSceneSpecificRoutine(int pSceneIndex)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) 
        {
            Debug.Log("woo fancy anim portal");
            EventBus<DarkenScreenEvent>.Publish(new DarkenScreenEvent());
        }
        else
        {
            EventBus<DarkenScreenEvent>.Publish(new DarkenScreenEvent());
        }
        yield return new WaitForSeconds(0.5f);

        // Check if scene to load is in bounds and then load it...
        if (pSceneIndex <= SceneManager.sceneCount && pSceneIndex >= 0)
        {
            SceneManager.LoadScene(pSceneIndex);
        }
        else { Debug.LogError("GameManager: Scene index " + pSceneIndex + " is invalid"); }
    }

    IEnumerator LoadSceneNextRoutine()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) 
        {
            Debug.Log("woo fancy anim portal");
            EventBus<DarkenScreenEvent>.Publish(new DarkenScreenEvent());
        }
        else
        {
            EventBus<DarkenScreenEvent>.Publish(new DarkenScreenEvent());
        }
        yield return new WaitForSeconds(0.5f);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if next index is valid to load, else reset it to zero...
        if (nextSceneIndex >= SceneManager.sceneCount) { nextSceneIndex = 0; }
        SceneManager.LoadScene(nextSceneIndex);

    }

}
