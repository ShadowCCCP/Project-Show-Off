using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFencing : MonoBehaviour
{
    [SerializeField]
    Slider enemyLife;
    [SerializeField]
    Slider playerLife;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.V))
        {
            UpdatePlayerLife(10);
        }
    }

    public void UpdateEnemyMaxLife(int maxLife)
    {
        enemyLife.maxValue = maxLife;
    }
    public void UpdatePlayerMaxLife(int maxLife)
    {
        playerLife.maxValue = maxLife;
    }

    public void UpdateEnemyLife(int life)
    {
        enemyLife.value = life;
    }
    public void UpdatePlayerLife(int life)
    {
        playerLife.value = life;

        animator.SetTrigger("TakeDamage");// plays the red screen

    }

}
