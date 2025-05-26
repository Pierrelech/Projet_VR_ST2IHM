using UnityEngine;
using TMPro;

public class GlobalScoreManager : MonoBehaviour
{
    public ContainerScore[] containers; // Référence à tous les conteneurs
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI totalErrorsText;

    void Update()
    {
        int totalScore = 0;
        int totalErrors = 0;

        foreach (ContainerScore container in containers)
        {
            totalScore += container.GetScore();
            totalErrors += container.GetErrors();
        }

        totalScoreText.text = "Score total : " + totalScore;
        totalErrorsText.text = "Erreurs totales : " + totalErrors;
    }
}
