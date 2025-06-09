using UnityEngine;

public class SessionManager : MonoBehaviour
{
    public PerformanceTracker performanceTracker; // référence au tracker global
    public GameObject endPanelUI; // UI affiché à la fin

    private bool sessionEnded = false;

    void Update()
    {
        if (!sessionEnded && AllDechetsCleared())
        {
            EndSession();
        }
    }

    bool AllDechetsCleared()
    {
        return GameObject.FindGameObjectsWithTag("DechetMarron").Length == 0 &&
               GameObject.FindGameObjectsWithTag("DechetVert").Length == 0 &&
               GameObject.FindGameObjectsWithTag("DechetJaune").Length == 0;
    }

    void EndSession()
    {
        sessionEnded = true;

        Debug.Log("✅ Tous les déchets triés ! Fin de session.");

        // Affiche les stats dans l’UI
        performanceTracker.DisplaySummary();

        // Active le panneau final
        if (endPanelUI != null)
        {
            endPanelUI.SetActive(true);
        }
    }

}
