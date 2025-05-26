using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisplayName : MonoBehaviour
{
    
    private bool isBeingHeld = false;
    private GameObject nameText;

    void Start()
    {
        nameText = transform.Find("NameText")?.gameObject;

        if (nameText != null)
        {
            nameText.SetActive(false); // Hide initially
        }
    }
    void Update()
    {
        if (nameText != null)
        {
            nameText.SetActive(isBeingHeld);
        }
    }

    public void OnGrab(SelectEnterEventArgs args)
    {
        isBeingHeld = true;
    }

    public void OnRelease(SelectExitEventArgs args)
    {
        isBeingHeld = false;
    }

}
