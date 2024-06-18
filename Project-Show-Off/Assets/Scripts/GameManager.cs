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

    public Vector3 PositionBeforeReset = new Vector3();

    [SerializeField]
    private int deathRoomIndex = 5;

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

        DontDestroyOnLoad(this.gameObject);

        EventBus<OnPlayerDeathEvent>.OnEvent += SaveOldPosAndDeath;
    }
    
    void OnDestroy()
    {
        EventBus<OnPlayerDeathEvent>.OnEvent -= SaveOldPosAndDeath;
    }

    // Preferred hand methods...
    public int GetDominantHand()
    {
        return dominantHand;
    }

    public string GetDominantHandString()
    {
        if(dominantHand == 0)
        {
            return "Left";
        }
        else
        {
            return "Right";
        }
    }

    public void SetDominantHand(int pDominatHand)
    {
        Debug.Log("Changed hand");
        EventBus<ChangeHandEvent>.Publish(new ChangeHandEvent());
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
        //reset saved position
        PositionBeforeReset = new Vector3(0, 0, 0);

        //do the transition animation
        if (pSceneIndex >= 0) 
        {
            Debug.Log("woo fancy anim portal");
            EventBus<DarkenScreenEvent>.Publish(new DarkenScreenEvent());
        }
        yield return new WaitForSeconds(2);

        // Check if scene to load is in bounds and then load it...
        if (pSceneIndex <= SceneManager.sceneCountInBuildSettings && pSceneIndex >= 0)
        {
            SceneManager.LoadScene(pSceneIndex);
        }
        else { Debug.Log("GameManager: Scene index " + pSceneIndex + " is invalid"); }
    }

    IEnumerator LoadSceneNextRoutine()
    {
        PositionBeforeReset = new Vector3 (0, 0, 0);
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

    public void SaveOldPosAndDeath(OnPlayerDeathEvent onPlayerDeathEvent)
    {
        Debug.Log("Player death");
        PositionBeforeReset = onPlayerDeathEvent.posDeath;

        LoadSceneSpecific(deathRoomIndex);
        setOffsetPos(onPlayerDeathEvent.posDeath);
    }

    void setOffsetPos(Vector3 deathPos)
    {
        
        PositionBeforeReset = new Vector3(deathPos.x, 0 , deathPos.z) ;
        Debug.Log(PositionBeforeReset);
    }

}
