using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Useful : MonoBehaviour
{
    public static string GetHierarchy(Transform pTransform)
    {
        Transform currentObject = pTransform;
        string hierarchy = "";
        List<string> names = new List<string>();

        // Iterate through each parent until none are left and add them to the list...
        while (currentObject.parent != null)
        {
            currentObject = currentObject.parent;
            names.Add(currentObject.name);
        }

        // Add all collected names into the string...
        for (int i = names.Count - 1; i >= 0; i--)
        {
            hierarchy += names[i] + " > ";
        }

        // And put the name that the script is attached to at the end...
        hierarchy += pTransform.name;

        return hierarchy;
    }
}
