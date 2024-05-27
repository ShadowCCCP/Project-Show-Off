using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class VRAbstractButton : MonoBehaviour
{
    Animator anim;
    bool canPressButton = true;
    [SerializeField]
    Animator glassAnim;

    bool locked;

    public float zPos;


    private void Start()
    {
        zPos = transform.position.z;
        anim = GetComponent<Animator>();

        if (glassAnim != null)
        {
            locked = true;
        }
    }
    private void Update()
    {
        transform.position = new Vector3(0,0,zPos);
    }

    private void OnTriggerEnter(Collider other)
    {


        if ((other.tag == "RightController" || other.tag == "LeftController") && canPressButton)
        {
            if (!locked)
            {


                anim.SetTrigger("Press");

                //action
                OnButtonPress();

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

    public abstract void OnButtonPress();

}
