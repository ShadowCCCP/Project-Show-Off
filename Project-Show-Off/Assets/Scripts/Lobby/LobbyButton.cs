using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyButton : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    int scene = -1;
    bool canPressButton = true;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if(other.tag == "Controller" && canPressButton)
        {
            anim.SetTrigger("Press");
            Debug.Log(SceneManager.sceneCount);
            if (scene > -1 && SceneManager.sceneCount >= scene)
            {
                SceneManager.LoadScene(scene);
            }
            else
            {
                Debug.Log("no scene");
            }
            canPressButton = false;
            StartCoroutine(buttonCooldown());
        }
    }

    IEnumerator buttonCooldown()
    {
        yield return new WaitForSeconds(0.5f);
        canPressButton = true;
    }    

    public void SetToGoScene(int i)
    {
        scene = i;
    }
}
