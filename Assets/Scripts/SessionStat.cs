using UnityEngine;
using TMPro;

public class SessionStats : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI errorsText;
    public TextMeshProUGUI timeText;

    public ContainerScore[] containers; // Référence à tous les conteneurs

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        timeText.text = "Temps : " + Mathf.FloorToInt(elapsedTime) + "s";

        UpdateGlobalStats();
    }

    void UpdateGlobalStats()
    {
        int totalScore = 0;
        int totalErrors = 0;

        foreach (ContainerScore container in containers)
        {
            totalScore += container.GetScore();
            totalErrors += container.GetErrors();
        }

        scoreText.text = "Score total : " + totalScore;
        errorsText.text = "Erreurs totales : " + totalErrors;
    }
}
