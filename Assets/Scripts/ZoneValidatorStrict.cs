using UnityEngine;

public class ZoneValidatorStrict : MonoBehaviour
{
    public Material idleMaterial;
    public Material successMaterial;
    public Renderer targetRenderer;

    private Collider zoneCollider;

    private void Start()
    {
        if (targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }

        zoneCollider = GetComponent<Collider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Grabbable") && zoneCollider != null)
        {
            if (IsFullyContained(other.bounds, zoneCollider.bounds))
            {
                targetRenderer.material = successMaterial;
            }
            else
            {
                targetRenderer.material = idleMaterial;
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

    private bool IsFullyContained(Bounds small, Bounds big)
    {
        return big.Contains(small.min) && big.Contains(small.max);
    }
}
