using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite : MonoBehaviour
{
    public Vector3 direction { set; private get; }
    public int speed { set; private get; }

    private void Start()
    {
        StartCoroutine(lifeTime());
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }


    IEnumerator lifeTime()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
