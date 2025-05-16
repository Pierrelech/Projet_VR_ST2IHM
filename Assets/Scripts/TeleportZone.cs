using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportZone : MonoBehaviour
{
    public Transform teleportTarget;   // L'endroit o� on veut t�l�porter
    public GameObject xrOrigin;        // Le XR Origin (joueur)

    public void TeleportPlayer()
    {
        if (xrOrigin != null && teleportTarget != null)
        {
            xrOrigin.transform.position = teleportTarget.position;
            xrOrigin.transform.rotation = teleportTarget.rotation;

            Debug.Log("Joueur t�l�port� !");
        }
    }
}
