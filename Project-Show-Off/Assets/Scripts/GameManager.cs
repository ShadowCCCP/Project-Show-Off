using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetDominantHand()
    {
        return dominantHand;
    }

    public void SetDominantHand(int pDominatHand)
    {
        Debug.Log("Changed hand");
        dominantHand = pDominatHand;
    }
}
