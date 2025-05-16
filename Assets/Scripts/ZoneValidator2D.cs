/*using UnityEngine;

public class ZoneValidator2D : MonoBehaviour
{
    public Material idleMaterial;
    public Material successMaterial;
    public Renderer targetRenderer;

    private Collider zoneCollider;

    private void Start()
    {
        zoneCollider = GetComponent<Collider>();

        if (targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Grabbable") || zoneCollider == null)
            return;

        Bounds zoneBounds = zoneCollider.bounds;
        Bounds objectBounds = other.bounds;

        // Vérifie si entièrement sur la surface (XZ)
        bool fullyInsideXZ =
            objectBounds.min.x >= zoneBounds.min.x &&
            objectBounds.max.x <= zoneBounds.max.x &&
            objectBounds.min.z >= zoneBounds.min.z &&
            objectBounds.max.z <= zoneBounds.max.z;

        // Active ou désactive le flag "can be placed"
        CubeInteractionTracker tracker = other.GetComponent<CubeInteractionTracker>();
        if (tracker != null)
        {
            tracker.canBePlaced = fullyInsideXZ;

            if (fullyInsideXZ && !tracker.isPlacedCorrectly && !tracker.IsBeingHeld())
            {
                tracker.MarkAsPlaced();
                if (targetRenderer != null && successMaterial != null)
                    targetRenderer.material = successMaterial;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable") && targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }
    }
}
*/

// Version 2

/* using UnityEngine;

public class ZoneValidator2D : MonoBehaviour
{
    public Material idleMaterial;
    public Material successMaterial;
    public Renderer targetRenderer;

    private Collider zoneCollider;

    private void Start()
    {
        zoneCollider = GetComponent<Collider>();

        if (targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }
    }

    

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Grabbable") || zoneCollider == null)
            return;

        Bounds zoneBounds = zoneCollider.bounds;
        Bounds objectBounds = other.bounds;

        bool fullyInsideXZ =
            objectBounds.min.x >= zoneBounds.min.x &&
            objectBounds.max.x <= zoneBounds.max.x &&
            objectBounds.min.z >= zoneBounds.min.z &&
            objectBounds.max.z <= zoneBounds.max.z;

        CubeInteractionTracker tracker = other.GetComponent<CubeInteractionTracker>();
        if (tracker == null)
            return;

        if (fullyInsideXZ && !tracker.IsBeingHeld())
        {
            // Si le cube n'est pas encore marqué comme placé
            if (!tracker.isPlacedCorrectly)
            {
                tracker.MarkAsPlaced();

                if (targetRenderer != null && successMaterial != null)
                    targetRenderer.material = successMaterial;
            }
            // Si déjà placé, on laisse la couleur de succès (ne rien changer)
        }
        else
        {
            // On ne remet le matériau Idle que si le cube n’est pas placé correctement
            if (!tracker.isPlacedCorrectly)
            {
                if (targetRenderer != null && idleMaterial != null)
                    targetRenderer.material = idleMaterial;
            }
        }
    }
       

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Grabbable")) return;

        if (targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }
    }
}
*/

// Version 3

/* using UnityEngine;

public class ZoneValidator2D : MonoBehaviour
{
    public Material idleMaterial;
    public Material successMaterial;
    public Renderer targetRenderer;

    public AudioSource audioSource;          // AudioSource à attacher
    public AudioClip successSound;           // Clip de réussite à jouer

    private Collider zoneCollider;

    private void Start()
    {
        zoneCollider = GetComponent<Collider>();

        if (targetRenderer != null && idleMaterial != null)
            targetRenderer.material = idleMaterial;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Grabbable") || zoneCollider == null) return;

        Bounds zoneBounds = zoneCollider.bounds;
        Bounds objectBounds = other.bounds;

        bool fullyInsideXZ =
            objectBounds.min.x >= zoneBounds.min.x &&
            objectBounds.max.x <= zoneBounds.max.x &&
            objectBounds.min.z >= zoneBounds.min.z &&
            objectBounds.max.z <= zoneBounds.max.z;

        CubeInteractionTracker tracker = other.GetComponent<CubeInteractionTracker>();
        if (tracker != null)
        {
            tracker.canBePlaced = fullyInsideXZ;

            if (fullyInsideXZ && !tracker.isPlacedCorrectly && !tracker.IsBeingHeld())
            {
                tracker.MarkAsPlaced();

                if (targetRenderer != null && successMaterial != null)
                    targetRenderer.material = successMaterial;

                // Play sound once
                if (audioSource != null && successSound != null)
                    audioSource.PlayOneShot(successSound);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable") && targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }
    }
}
*/

// Version 4

using UnityEngine;

public class ZoneValidator2D : MonoBehaviour
{
    public Material idleMaterial;
    public Material successMaterial;
    public Renderer targetRenderer;

    public AudioSource audioSource;          // AudioSource à attacher
    public AudioClip successSound;           // Clip de réussite à jouer

    private Collider zoneCollider;

    private void Start()
    {
        zoneCollider = GetComponent<Collider>();

        if (targetRenderer != null && idleMaterial != null)
            targetRenderer.material = idleMaterial;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Grabbable") || zoneCollider == null) return;

        CubeInteractionTracker tracker = other.GetComponent<CubeInteractionTracker>();
        if (tracker == null) return;

        bool fullyInsideXZ = AreBottomCornersInsideXZ(other.bounds, zoneCollider.bounds);
        
        tracker.canBePlaced = fullyInsideXZ;

        if (fullyInsideXZ && !tracker.isPlacedCorrectly && !tracker.IsBeingHeld())
        {
            tracker.MarkAsPlaced();

            if (targetRenderer != null && successMaterial != null)
                targetRenderer.material = successMaterial;

            if (audioSource != null && successSound != null)
                audioSource.PlayOneShot(successSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable") && targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }
    }

    // Nouvelle méthode : vérifie les 4 coins inférieurs (XZ uniquement)
    private bool AreBottomCornersInsideXZ(Bounds objectBounds, Bounds zoneBounds)
    {
        Vector3[] bottomCorners = new Vector3[4];

        float y = objectBounds.min.y; // bas du cube
        float minX = objectBounds.min.x;
        float maxX = objectBounds.max.x;
        float minZ = objectBounds.min.z;
        float maxZ = objectBounds.max.z;

        bottomCorners[0] = new Vector3(minX, y, minZ);
        bottomCorners[1] = new Vector3(minX, y, maxZ);
        bottomCorners[2] = new Vector3(maxX, y, minZ);
        bottomCorners[3] = new Vector3(maxX, y, maxZ);

        foreach (var corner in bottomCorners)
        {
            if (!zoneBounds.Contains(corner))
                return false;
        }

        return true;
    }

    /*private bool AreBottomCornersInsideZoneSurface(Transform cubeTransform, Bounds cubeBounds, Transform zoneTransform)
    {
        Vector3[] bottomCorners = new Vector3[4];

        float y = cubeBounds.min.y;
        float minX = cubeBounds.min.x;
        float maxX = cubeBounds.max.x;
        float minZ = cubeBounds.min.z;
        float maxZ = cubeBounds.max.z;

        bottomCorners[0] = new Vector3(minX, y, minZ);
        bottomCorners[1] = new Vector3(minX, y, maxZ);
        bottomCorners[2] = new Vector3(maxX, y, minZ);
        bottomCorners[3] = new Vector3(maxX, y, maxZ);

        foreach (var corner in bottomCorners)
        {
            // On transforme chaque coin dans l'espace local de la zone
            Vector3 localPoint = zoneTransform.InverseTransformPoint(corner);

            // On vérifie s'il tombe dans la zone XZ de -0.5 à +0.5 (plane de scale 1)
            if (Mathf.Abs(localPoint.x) > 0.5f || Mathf.Abs(localPoint.z) > 0.5f)
                return false;
        }

        return true;
    }*/

    /*private bool AreBottomCornersInsideZoneSurface(Transform cubeTransform, Bounds cubeBounds, Transform zoneTransform)
    {
        Vector3[] bottomCorners = new Vector3[4];

        float y = cubeBounds.min.y;
        float minX = cubeBounds.min.x;
        float maxX = cubeBounds.max.x;
        float minZ = cubeBounds.min.z;
        float maxZ = cubeBounds.max.z;

        bottomCorners[0] = new Vector3(minX, y, minZ);
        bottomCorners[1] = new Vector3(minX, y, maxZ);
        bottomCorners[2] = new Vector3(maxX, y, minZ);
        bottomCorners[3] = new Vector3(maxX, y, maxZ);

        Vector3 zoneSize = zoneTransform.localScale;

        foreach (var corner in bottomCorners)
        {
            // Passe le point dans le repère local de la zone
            Vector3 localPoint = zoneTransform.InverseTransformPoint(corner);

            // Vérifie les coordonnées en tenant compte du scale
            if (Mathf.Abs(localPoint.x) > 0.5f * zoneSize.x ||
                Mathf.Abs(localPoint.z) > 0.5f * zoneSize.z)
                return false;
        }

        return true;
    }*/

}