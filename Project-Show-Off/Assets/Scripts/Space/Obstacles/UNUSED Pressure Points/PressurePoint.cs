using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePoint : MonoBehaviour
{
    [SerializeField]
    ParticleSystem smoke;
    [SerializeField]
    ParticleSystem warningSmoke;

    Vector3 startPos;
    Vector3 endPos;
    void Start()
    {
        smoke.Stop();

        startPos = transform.position;
        endPos = startPos - (transform.right * 1);
    }


    public void OnPush()
    {
        StartCoroutine(smokeDelay());
    }

    public void OnWarning()
    {
        warningSmoke.Play();
    }

    public void Retreat()
    {
        smoke.Stop();
        LeanTween.move(gameObject, startPos, 1);
    }

    IEnumerator smokeDelay()
    {
        warningSmoke.Stop();
        smoke.Play();
        yield return new WaitForSeconds(1);
        LeanTween.move(gameObject, endPos, 1);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            //allow player to use physics and be pushed around
            EventBus<StopPlayerMovementEvent>.Publish(new StopPlayerMovementEvent());
        }
    }
}
