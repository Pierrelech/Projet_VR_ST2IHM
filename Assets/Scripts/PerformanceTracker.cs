using UnityEngine;
using TMPro;

public class PerformanceTracker : MonoBehaviour
{
    public SessionStats sessionStats; // référence aux conteneurs
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI errorsText;

    private float totalTime = 0f;

    void Update()
    {
        totalTime += Time.deltaTime;
    }

    public void DisplaySummary()
    {
        int totalScore = 0;
        int totalErrors = 0;

        foreach (var container in sessionStats.containers)
        {
            totalScore += container.GetScore();
            totalErrors += container.GetErrors();
        }

        timeText.text = "Temps écoulé : " + Mathf.FloorToInt(totalTime) + "s";
        scoreText.text = "Score total : " + totalScore;
        errorsText.text = "Erreurs totales : " + totalErrors;
    }
}
