using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyButton : MonoBehaviour
{
    Animator anim;
    [SerializeField]
    int scene = -1;
    bool canPressButton = true;
    [SerializeField]
    Animator glassAnim;

    bool locked;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        if(glassAnim != null)
        {
            locked = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        

        if(other.tag == "Controller" && canPressButton )
        {
            if (!locked)
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
                StartCoroutine(buttonCooldown(0.5f));
            }
            else
            {
                glassAnim.SetTrigger("Break");
                StartCoroutine(buttonCooldown(1f));
            }
        }
    }

    IEnumerator buttonCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canPressButton = true;
        if (locked)
        {
            locked = false;
            glassAnim.gameObject.SetActive(false);
        }

    }    

    public void SetToGoScene(int i)
    {
        scene = i;
    }
}
