using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    int life = 100;
    [SerializeField]
    Transform cameraTranform;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rb.velocity += new Vector3(0, -1, 0);
    }

    private void Update()
    { 
        transform.position = new Vector3( cameraTranform.position.x, transform.position.y , cameraTranform.position.z);
        //cameraTranform.position = new Vector3(cameraTranform.position.x, transform.position.y, cameraTranform.position.z);

        Debug.Log(rb.velocity.y);
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
