/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastGrab : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction; // Grip
    public Transform attachPoint;

    private GameObject heldObject = null;

    void Update()
    {
        if (heldObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform.GetComponent<XRGrabInteractable>())
                {
                    heldObject = hit.transform.gameObject;
                    heldObject.transform.position = attachPoint.position;
                    heldObject.transform.rotation = attachPoint.rotation;
                }
            }
        }

        if (heldObject != null && activateAction.action.WasReleasedThisFrame())
        {
            heldObject = null;
        }
    }
}
*/
// Version 2

/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastGrab : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction; // Grip
    public Transform attachPoint;

    private GameObject heldObject = null;
    private Rigidbody heldRigidbody = null;

    void Update()
    {
        // Début du grab
        if (heldObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    heldObject = hit.transform.gameObject;
                    heldObject.transform.position = attachPoint.position;
                    heldObject.transform.rotation = attachPoint.rotation;

                    // Gestion de la gravité pendant le grab
                    heldRigidbody = heldObject.GetComponent<Rigidbody>();
                    if (heldRigidbody != null)
                    {
                        heldRigidbody.isKinematic = true;
                    }
                }
            }
        }

        // Pendant le grab (suivre le point d'attache)
        if (heldObject != null && activateAction.action.IsPressed())
        {
            heldObject.transform.position = attachPoint.position;
            heldObject.transform.rotation = attachPoint.rotation;
        }

        // Fin du grab
        if (heldObject != null && activateAction.action.WasReleasedThisFrame())
        {
            // Réactiver la physique
            if (heldRigidbody != null)
            {
                heldRigidbody.isKinematic = false;
                heldRigidbody = null;
            }

            heldObject = null;
        }
    }
}
*/

// Version 3
/*
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastGrab : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction; // Grip
    public Transform attachPoint;
    public ActionBasedContinuousMoveProvider moveProvider;

    private GameObject heldObject = null;
    private Rigidbody heldRigidbody = null;

    void Update()
    {
        // Début du grab
        if (heldObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform.GetComponent<XRGrabInteractable>())
                {
                    heldObject = hit.transform.gameObject;
                    heldObject.transform.position = attachPoint.position;
                    heldObject.transform.rotation = attachPoint.rotation;

                    heldRigidbody = heldObject.GetComponent<Rigidbody>();
                    if (heldRigidbody != null)
                        heldRigidbody.isKinematic = true;

                    if (moveProvider != null)
                        moveProvider.enabled = false;

                    // Appelle OnGrab manuellement
                    CubeInteractionTracker tracker = heldObject.GetComponent<CubeInteractionTracker>();
                    if (tracker != null)
                        tracker.OnGrab(null); // on passe "null" car on n'a pas d'args
                }
            }
        }

        // Suivi
        if (heldObject != null && activateAction.action.IsPressed())
        {
            heldObject.transform.position = attachPoint.position;
            heldObject.transform.rotation = attachPoint.rotation;
        }

        // Fin du grab
        if (heldObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (heldRigidbody != null)
            {
                heldRigidbody.isKinematic = false;
                heldRigidbody = null;

                CubeInteractionTracker tracker = heldObject.GetComponent<CubeInteractionTracker>();
                if (tracker != null)
                    tracker.OnRelease(null);
            }

            heldObject = null;

            if (moveProvider != null)
                moveProvider.enabled = true;
        }
    }
}
*/
// Verion 4

/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastGrab : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction; // Grip
    public Transform attachPoint;

    private GameObject heldObject = null;
    private CubeInteractionTracker currentTracker = null;

    void Update()
    {
        // Début du grab
        if (heldObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform.GetComponent<XRGrabInteractable>())
                {
                    heldObject = hit.transform.gameObject;

                    // Appelle OnGrab sur le tracker
                    currentTracker = heldObject.GetComponent<CubeInteractionTracker>();
                    if (currentTracker != null)
                        currentTracker.OnGrab(null);

                    // Positionne l’objet
                    heldObject.transform.position = attachPoint.position;
                    heldObject.transform.rotation = attachPoint.rotation;
                }
            }
        }

        // Pendant le grab : l’objet suit l’attachPoint
        if (heldObject != null)
        {
            heldObject.transform.position = attachPoint.position;
            heldObject.transform.rotation = attachPoint.rotation;
        }

        // Fin du grab
        if (heldObject != null && activateAction.action.WasReleasedThisFrame())
        {
            // Appelle OnRelease sur le tracker
            if (currentTracker != null)
                currentTracker.OnRelease(null);

            heldObject = null;
            currentTracker = null;
        }
    }
}
*/

// Version 5

/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastGrab : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction; // Grip
    public Transform attachPoint;
    public ContinuousMoveProviderBase moveProvider; // déplacement XR à désactiver

    private GameObject heldObject = null;
    private CubeInteractionTracker currentTracker = null;

    void Update()
    {
        // Début du grab
        if (heldObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                if (hit.transform.GetComponent<XRGrabInteractable>())
                {
                    heldObject = hit.transform.gameObject;

                    currentTracker = heldObject.GetComponent<CubeInteractionTracker>();
                    if (currentTracker != null)
                        currentTracker.OnGrab(null);

                    heldObject.transform.position = attachPoint.position;
                    heldObject.transform.rotation = attachPoint.rotation;

                    // Désactiver le déplacement XR
                    if (moveProvider != null)
                        moveProvider.enabled = false;
                }
            }
        }

        // Pendant le grab
        if (heldObject != null)
        {
            heldObject.transform.position = attachPoint.position;
            heldObject.transform.rotation = attachPoint.rotation;
        }

        // Fin du grab
        if (heldObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (currentTracker != null)
                currentTracker.OnRelease(null);

            heldObject = null;
            currentTracker = null;

            // Réactiver le déplacement XR
            if (moveProvider != null)
                moveProvider.enabled = true;
        }
    }
}
*/

// Version 6
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastGrab : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;
    public Transform attachPoint;
    public ContinuousMoveProviderBase moveProvider;

    private GameObject heldObject = null;
    private Rigidbody heldRigidbody = null;
    private CubeInteractionTracker currentTracker = null;

    void Update()
    {
        if (heldObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    heldObject = hit.transform.gameObject;
                    heldRigidbody = heldObject.GetComponent<Rigidbody>();

                    // Désactiver la physique
                    if (heldRigidbody != null)
                    {
                        heldRigidbody.isKinematic = true;
                        heldRigidbody.useGravity = false;
                    }

                    // Tracker
                    currentTracker = heldObject.GetComponent<CubeInteractionTracker>();
                    if (currentTracker != null)
                        currentTracker.OnGrab(null);

                    // Positionner l'objet au point de collision
                    heldObject.transform.position = attachPoint.position;
                    heldObject.transform.rotation = attachPoint.rotation;

                }
            }
        }

        // Suivi pendant le grab
        if (heldObject != null)
        {
            heldObject.transform.position = attachPoint.position;
            heldObject.transform.rotation = attachPoint.rotation;
        }

        // Fin du grab
        if (heldObject != null && activateAction.action.WasReleasedThisFrame())
        {
            // Réactiver la physique
            if (heldRigidbody != null)
            {
                heldRigidbody.isKinematic = false;
                heldRigidbody.useGravity = true;
            }

            // Tracker
            if (currentTracker != null)
                currentTracker.OnRelease(null);

            // Réactiver le déplacement XR
            if (moveProvider != null)
                moveProvider.enabled = true;

            // Reset
            heldObject = null;
            heldRigidbody = null;
            currentTracker = null;
        }
    }
}
