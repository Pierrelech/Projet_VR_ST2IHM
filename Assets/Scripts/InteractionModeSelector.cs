using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionModeSelector : MonoBehaviour
{
    public GameObject grabInteractionObject; // contient RaycastGrab.cs
    public GameObject dragInteractionObject; // contient RaycastDrag.cs

    public XRRayInteractor rayInteractor; // le ray interactor reste actif dans tous les cas


    public enum Mode { Grab, Drag, Native }
    public Mode currentMode = Mode.Native;

    void Start()
    {
        SetNative(); // on démarre en mode natif par défaut
    }

    public void SetGrab()
    {
        currentMode = Mode.Grab;
        ApplyMode();
    }

    public void SetDrag()
    {
        currentMode = Mode.Drag;
        ApplyMode();
    }

    public void SetNative()
    {
        currentMode = Mode.Native;
        ApplyMode();
    }

    void ApplyMode()
    {
        grabInteractionObject.SetActive(currentMode == Mode.Grab);
        dragInteractionObject.SetActive(currentMode == Mode.Drag);
        // XR Ray Interactor reste toujours activé
        if (rayInteractor != null)
        {
            // On bloque la sélection native sauf en mode Natif
            rayInteractor.allowSelect = (currentMode == Mode.Native);
        }
    }
}


// Version 2
/*
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractionModeSelector : MonoBehaviour
{
    public GameObject grabInteractionObject;
    public GameObject dragInteractionObject;

    public XRRayInteractor rayInteractor;

    public CubeInteractionTracker[] allTrackers;  // Ajout des cubes à reset

    public enum Mode { NativeGrab, Grab, Drag }
    private Mode currentMode = Mode.NativeGrab;

    public void SetModeToNative()
    {
        currentMode = Mode.NativeGrab;
        ApplyMode();
    }

    public void SetModeToGrab()
    {
        currentMode = Mode.Grab;
        ApplyMode();
    }

    public void SetModeToDrag()
    {
        currentMode = Mode.Drag;
        ApplyMode();
    }

    void ApplyMode()
    {
        grabInteractionObject.SetActive(currentMode == Mode.Grab);
        dragInteractionObject.SetActive(currentMode == Mode.Drag);

        // Active/désactive la sélection native
        if (rayInteractor != null)
        {
            rayInteractor.allowSelect = (currentMode == Mode.NativeGrab);
        }

        //  Reset tous les trackers
        ResetTrackers();
    }

    void ResetTrackers()
    {
        foreach (var tracker in allTrackers)
        {
            if (tracker != null)
            {
                tracker.ResetTracker();
            }
        }
    }
}
*/