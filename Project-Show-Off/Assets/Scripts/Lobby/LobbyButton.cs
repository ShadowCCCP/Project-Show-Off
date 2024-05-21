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
 
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Controller" && canPressButton)
        {
            anim.SetTrigger("Press");
            if (scene > -1)
            {
                SceneManager.LoadScene(scene);
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
}
