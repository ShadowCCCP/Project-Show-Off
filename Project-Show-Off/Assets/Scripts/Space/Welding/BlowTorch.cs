using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.XR.Interaction.Toolkit;

public class BlowTorch : MonoBehaviour
{
    [SerializeField]
    ParticleSystem sparks;
    [SerializeField]
    ParticleSystem smoke;
    [SerializeField]
    ParticleSystem flame;

    Collider coll;

    [SerializeField]
    ActionBasedController rightHand;
    [SerializeField]
    ActionBasedController leftHand;

    [SerializeField]
    SkinnedMeshRenderer rightMesh;
    [SerializeField]
    SkinnedMeshRenderer leftMesh;

    [SerializeField]
    GameObject blowtorch;


    ActionBasedController currentController;


    private void Start()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
        sparks.Stop();
        smoke.Stop();

        if (GameManager.Instance)
        {

            setHand(GameManager.Instance.GetDominantHand());
        }
        else
        {
            setHand(1);
        }
    }

    private void Update()
    {

        if (currentController.activateAction.action.ReadValue<float>() > 0.5f)
        {
            activateFire();
        }
        else
        {
            deactivateFire();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<WeldableCube>() != null)
        {
            sparks.Play();
            smoke.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<WeldableCube>() != null)
        {
            sparks.Stop();
            smoke.Stop();
        }
    }

    void activateFire()
    {
        coll.enabled = true;
        flame.Play();
    }

    void deactivateFire()
    {
        coll.enabled = false;
        flame.Stop();
    }

    private void setHand(int hand)
    {
        if (hand == 0)
        {
            currentController = leftHand;
            blowtorch.transform.SetParent(leftHand.transform);
            rightMesh.enabled = true;
            leftMesh.enabled = false;
        }
        else
        {
            currentController = rightHand;
            blowtorch.transform.SetParent(rightHand.transform);
            rightMesh.enabled = false;
            leftMesh.enabled = true;
        }
    }

}
