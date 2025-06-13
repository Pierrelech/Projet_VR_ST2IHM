//Version 1
/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;

    private Transform draggedObject = null;
    private Plane dragPlane; // Plan sur lequel on déplace l'objet

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;

                    // Crée un plan parallèle à la caméra au niveau de l'objet
                    Vector3 planeNormal = -rayInteractor.rayOriginTransform.forward;
                    dragPlane = new Plane(planeNormal, draggedObject.position);
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);
                draggedObject.position = targetPosition;
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            draggedObject = null;
        }
    }
}
*/
//Version 2
/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;

    private Transform draggedObject = null;
    private Rigidbody draggedRigidbody = null;
    private Plane dragPlane;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;

                    draggedRigidbody = draggedObject.GetComponent<Rigidbody>();
                    if (draggedRigidbody != null)
                    {
                        draggedRigidbody.isKinematic = true; // désactiver la physique pendant le drag
                    }

                    // Créer un plan à la profondeur de l'objet
                    Vector3 planeNormal = -rayInteractor.rayOriginTransform.forward;
                    dragPlane = new Plane(planeNormal, draggedObject.position);
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);
                draggedObject.position = targetPosition;
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (draggedRigidbody != null)
            {
                draggedRigidbody.isKinematic = false; // réactiver la physique après le drag
                draggedRigidbody = null;
            }

            draggedObject = null;
        }
    }
}
*/

// Version 3

/*using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;

    [Tooltip("Distance maximale que l'objet peut parcourir depuis sa position d'origine")]
    public float maxDragDistance = 2.0f;

    private Transform draggedObject = null;
    private Rigidbody draggedRigidbody = null;
    private Plane dragPlane;
    private Vector3 initialDragPosition;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;

                    draggedRigidbody = draggedObject.GetComponent<Rigidbody>();
                    if (draggedRigidbody != null)
                        draggedRigidbody.isKinematic = true;

                    // Plan horizontal à la hauteur de l'objet
                    dragPlane = new Plane(Vector3.up, draggedObject.position);
                    initialDragPosition = draggedObject.position;
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);

                // Vérifier la distance maximale
                float distance = Vector3.Distance(initialDragPosition, targetPosition);
                if (distance <= maxDragDistance)
                {
                    draggedObject.position = targetPosition;
                }
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (draggedRigidbody != null)
            {
                draggedRigidbody.isKinematic = false;
                draggedRigidbody = null;
            }

            draggedObject = null;
        }
    }
}
*/

// Version 4

/* using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;
    public float maxDragDistance = 2.0f;
    public ActionBasedContinuousMoveProvider moveProvider;

    private Transform draggedObject = null;
    private Rigidbody draggedRigidbody = null;
    private Plane dragPlane;
    private Vector3 initialDragPosition;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;
                    draggedRigidbody = draggedObject.GetComponent<Rigidbody>();

                    if (draggedRigidbody != null)
                        draggedRigidbody.isKinematic = true;

                    dragPlane = new Plane(Vector3.up, draggedObject.position);
                    initialDragPosition = draggedObject.position;

                    if (moveProvider != null)
                        moveProvider.enabled = false;
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);
            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);
                float distance = Vector3.Distance(initialDragPosition, targetPosition);

                if (distance <= maxDragDistance)
                {
                    draggedObject.position = targetPosition;
                }
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (draggedRigidbody != null)
            {
                draggedRigidbody.isKinematic = false;
                draggedRigidbody = null;
            }

            draggedObject = null;

            if (moveProvider != null)
                moveProvider.enabled = true;
        }
    }
}
*/

// Version 5

/* using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;

    private Transform draggedObject = null;
    private Plane dragPlane;
    private CubeInteractionTracker tracker = null;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;
                    tracker = draggedObject.GetComponent<CubeInteractionTracker>();

                    // Crée un plan horizontal à la hauteur du cube
                    Vector3 planeNormal = -rayInteractor.rayOriginTransform.forward;
                    dragPlane = new Plane(planeNormal, draggedObject.position);

                    // Appel du tracker (début chrono + essais)
                    if (tracker != null)
                        tracker.OnGrab(null);
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);
                draggedObject.position = targetPosition;
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (tracker != null)
                tracker.OnRelease(null);

            draggedObject = null;
            tracker = null;
        }
    }
}
*/
// Version 6

/* using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;

    private Transform draggedObject = null;
    private Rigidbody draggedRigidbody = null;
    private CubeInteractionTracker tracker = null;
    private Plane dragPlane;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;
                    draggedRigidbody = draggedObject.GetComponent<Rigidbody>();

                    // Kinematic pendant le drag
                    if (draggedRigidbody != null)
                    {
                        draggedRigidbody.isKinematic = true;
                        draggedRigidbody.useGravity = false;
                    }

                    tracker = draggedObject.GetComponent<CubeInteractionTracker>();
                    if (tracker != null)
                        tracker.OnGrab(null);

                    // Créer un plan horizontal à la hauteur de l'objet
                    Vector3 planeNormal = -rayInteractor.rayOriginTransform.forward;
                    dragPlane = new Plane(planeNormal, draggedObject.position);
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);
                draggedObject.position = targetPosition;
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (tracker != null)
                tracker.OnRelease(null);

            // Rétablir la physique
            if (draggedRigidbody != null)
            {
                draggedRigidbody.isKinematic = false;
                draggedRigidbody.useGravity = true;
            }

            draggedObject = null;
            draggedRigidbody = null;
            tracker = null;
        }
    }
}
*/

// Version 7

/* using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;

    private Transform draggedObject = null;
    private Rigidbody draggedRigidbody = null;
    private CubeInteractionTracker tracker = null;

    private Plane dragPlane;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;
                    draggedRigidbody = draggedObject.GetComponent<Rigidbody>();
                    tracker = draggedObject.GetComponent<CubeInteractionTracker>();

                    // On désactive la physique pendant le drag
                    if (draggedRigidbody != null)
                    {
                        draggedRigidbody.isKinematic = true;
                        draggedRigidbody.useGravity = false;
                    }

                    // On informe le tracker que le drag commence
                    if (tracker != null)
                        tracker.OnGrab(null);

                    // On définit un plan horizontal à la hauteur du cube
                    Vector3 planeNormal = -rayInteractor.rayOriginTransform.forward;
                    dragPlane = new Plane(planeNormal, draggedObject.position);
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);
                draggedObject.position = targetPosition;
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (tracker != null)
                tracker.OnRelease(null);

            // On réactive la physique normale
            if (draggedRigidbody != null)
            {
                draggedRigidbody.isKinematic = false;
                draggedRigidbody.useGravity = true;
            }

            // Reset
            draggedObject = null;
            draggedRigidbody = null;
            tracker = null;
        }
    }
}
*/

// Version 8

/* using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;
    public ContinuousMoveProviderBase moveProvider; // pour désactiver le move

    private Transform draggedObject = null;
    private Rigidbody draggedRigidbody = null;
    private CubeInteractionTracker tracker = null;
    private Plane dragPlane;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;
                    draggedRigidbody = draggedObject.GetComponent<Rigidbody>();
                    tracker = draggedObject.GetComponent<CubeInteractionTracker>();

                    // On désactive la physique pendant le drag
                    if (draggedRigidbody != null)
                    {
                        draggedRigidbody.isKinematic = true;
                        draggedRigidbody.useGravity = false;
                    }

                    // Démarrage du chrono
                    if (tracker != null)
                        tracker.OnGrab(null);

                    // Plan horizontal (XZ) à la hauteur du cube
                    dragPlane = new Plane(Vector3.up, draggedObject.position);

                    // Désactivation du mouvement XR
                    if (moveProvider != null)
                        moveProvider.enabled = false;
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);
                draggedObject.position = targetPosition;
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {
            if (tracker != null)
                tracker.OnRelease(null);

            if (draggedRigidbody != null)
            {
                draggedRigidbody.isKinematic = false;
                draggedRigidbody.useGravity = true;
            }

            // Réactiver le move
            if (moveProvider != null)
                moveProvider.enabled = true;

            draggedObject = null;
            draggedRigidbody = null;
            tracker = null;
        }
    }
}
*/

// Version 9

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class RaycastDrag : MonoBehaviour
{
    [Header("Références")]
    public XRRayInteractor rayInteractor;
    public InputActionProperty activateAction;
    public ContinuousMoveProviderBase moveProvider;
    public InputActionProperty leftJoystickAction; // (Vector2)


    [Header("Limite de déplacement")]
    public float maxDistance = 2f; // Valeur modifiable dans l’Inspector
    public float maxHauteur = 80f; // Hauteur maximum (modifiable)

    private Transform draggedObject = null;
    private Rigidbody draggedRigidbody = null;

    private Plane dragPlane;
    private Vector3 dragStartPosition;

    void Update()
    {
        // Début du drag
        if (draggedObject == null && activateAction.action.WasPressedThisFrame())
        {
            if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            {
                XRGrabInteractable interactable = hit.transform.GetComponent<XRGrabInteractable>();
                if (interactable != null)
                {
                    draggedObject = hit.transform;
                    draggedRigidbody = draggedObject.GetComponent<Rigidbody>();

                    // Désactiver la physique pendant le drag
                    if (draggedRigidbody != null)
                    {
                        draggedRigidbody.isKinematic = true;
                        draggedRigidbody.useGravity = false;
                    }

                    // Plan horizontal (XZ)
                    dragPlane = new Plane(Vector3.up, draggedObject.position);
                    dragStartPosition = draggedObject.position;

                    // Désactiver le mouvement joystick
                    if (moveProvider != null)
                        moveProvider.enabled = false;
                }
            }
        }

        // Pendant le drag
        if (draggedObject != null && activateAction.action.IsPressed())
        {
            Ray ray = new Ray(rayInteractor.rayOriginTransform.position, rayInteractor.rayOriginTransform.forward);

            if (dragPlane.Raycast(ray, out float enter))
            {
                Vector3 targetPosition = ray.GetPoint(enter);

                // Limite de distance
                Vector3 offset = targetPosition - dragStartPosition;
                if (offset.magnitude > maxDistance)
                    targetPosition = dragStartPosition + offset.normalized * maxDistance;

                // Début du script déplacement vertical de l'objet grâce au Joystick gauche
                // Lecture de l’axe vertical du joystick gauche
                float verticalInput = leftJoystickAction.action.ReadValue<Vector2>().y;


                // Appliquer le décalage vertical avec Time.deltaTime pour que ce soit fluide
                targetPosition.y += verticalInput * maxHauteur * Time.deltaTime;

                draggedObject.position = targetPosition;
                // Fin du script de déplacement vertical
            }
        }

        // Fin du drag
        if (draggedObject != null && activateAction.action.WasReleasedThisFrame())
        {

            // Restauration de la physique
            if (draggedRigidbody != null)
            {
                draggedRigidbody.isKinematic = false;
                draggedRigidbody.useGravity = true;
            }

            // Réactiver le Move
            if (moveProvider != null)
                moveProvider.enabled = true;

            // Reset
            draggedObject = null;
            draggedRigidbody = null;
        }
    }
}
