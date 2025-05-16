using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportZone : MonoBehaviour
{
    public Transform teleportTarget;   // L'endroit où on veut téléporter
    public GameObject xrOrigin;        // Le XR Origin (joueur)

    public void TeleportPlayer()
    {
        if (xrOrigin != null && teleportTarget != null)
        {
            xrOrigin.transform.position = teleportTarget.position;
            xrOrigin.transform.rotation = teleportTarget.rotation;

            Debug.Log("Joueur téléporté !");
        }
    }
}
