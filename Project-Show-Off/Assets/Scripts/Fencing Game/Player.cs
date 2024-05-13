using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    int life = 100;
    [SerializeField]
    Transform cameraTranform;

    private void Update()
    {
        transform.position = new Vector3( cameraTranform.position.x, transform.position.y , cameraTranform.position.z);
        cameraTranform.position = new Vector3(cameraTranform.position.x, transform.position.y, cameraTranform.position.z);
    }

    public void TakeDamage(int dmg)
    {
        life -= dmg;

        if (life < 0)
        {
            playerDeath();
        }
    }

    void playerDeath()
    {
        Debug.Log("restart game");
    }


    
}
