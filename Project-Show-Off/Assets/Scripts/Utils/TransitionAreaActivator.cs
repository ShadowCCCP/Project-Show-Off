using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TransitionAreaActivator : MonoBehaviour
{
    [SerializeField] GameObject transitionArea;

    [Tooltip("Amount of time before the area actually appears.")]
    [SerializeField] float bufferTime = 5;

    [SerializeField] XRGrabInteractable[] grabToTrigger;

    private List<IXRSelectInteractable> interactedWith = new List<IXRSelectInteractable>();

    private void Start()
    {
        // Check for potential error causes...
        if (transitionArea != null)
        {
            if (transitionArea.activeSelf)
            {
                transitionArea.SetActive(false);
            }
        }
        else { Debug.LogError(Useful.GetHierarchy(transform) + "\nTransitionAreaActivator: No transitionArea attached to the script.");}

        // Listen to every XRGrabInteractable...
        for (int i = 0; i < grabToTrigger.Length; i++)
        {
            grabToTrigger[i].selectEntered.AddListener(NoteGrabbedObject);
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < grabToTrigger.Length; i++)
        {
            grabToTrigger[i].selectEntered.RemoveListener(NoteGrabbedObject);
        }
    }

    private void NoteGrabbedObject(SelectEnterEventArgs args)
    {
        // If interactable wasn't noted already...
        if (!interactedWith.Contains(args.interactableObject))
        {
            // Add it to the list...
            interactedWith.Add(args.interactableObject);

            // If all interactables were grabbed, activate transition zone...
            if (interactedWith.Count == grabToTrigger.Length)
            {
                TriggerDialogueEvents();
                StartCoroutine(SpawnTransitionArea());
            }
        }
    }

    private void TriggerDialogueEvents()
    {
        EventBus<OnPencilPickupEvent>.Publish(new OnPencilPickupEvent());
    }

    private IEnumerator SpawnTransitionArea()
    {
        yield return new WaitForSeconds(bufferTime);

        transitionArea.SetActive(true);
    }
}
