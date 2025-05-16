using UnityEngine;

public class ZoneValidator : MonoBehaviour
{
    public Material idleMaterial;
    public Material successMaterial;
    public Renderer targetRenderer;

    private void Start()
    {
        if (targetRenderer != null && idleMaterial != null)
        {
            targetRenderer.material = idleMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            if (targetRenderer != null && successMaterial != null)
            {
                targetRenderer.material = successMaterial;
            }

            Debug.Log($" {other.name} is inside TargetZone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
        {
            if (targetRenderer != null && idleMaterial != null)
            {
                targetRenderer.material = idleMaterial;
            }

            Debug.Log($" {other.name} exited TargetZone.");
        }
    }
}
