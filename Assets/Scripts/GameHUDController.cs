using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameHUDController : MonoBehaviour
{
    [Header("In-Game UI")]
    public TMP_Text scoreText;
    public TMP_Text errorText;
    public TMP_Text timerText;

    [Header("End Screen UI")]
    public CanvasGroup endScreenGroup;
    public GameObject endScreenCanvas; // Le GameObject Canvas parent
    public TMP_Text titleText;
    public TMP_Text finalScoreText;
    public TMP_Text finalErrorsText;
    public TMP_Text finalTimeText;

    private int score = 0;
    private int errors = 0;
    private float timeElapsed = 0f;
    private bool sessionEnded = false;

    void Update()
    {
        if (score + errors > 10) sessionEnded = true;
        if (sessionEnded) EndSession(sessionEnded);

        timeElapsed += Time.deltaTime;
        UpdateHUD();
    }

    public void AddScore()
    {
        score ++;
        UpdateHUD();
    }

    public void AddError()
    {
        errors++;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        scoreText.text = $"Score : {score}";
        errorText.text = $"Erreurs : {errors}";
        timerText.text = $"Temps : {Mathf.FloorToInt(timeElapsed)}s";
    }

    public void EndSession(bool success)
    {
        sessionEnded = true;

        // Remplir les valeurs de l'écran final
        titleText.text = success ? "Bravo ! 🎉" : "Session terminée";
        finalScoreText.text = $"Score final : {score}";
        finalErrorsText.text = $"Nombre d'erreurs : {errors}";
        finalTimeText.text = $"Temps total : {Mathf.FloorToInt(timeElapsed)}s";

        // Activer et afficher l'écran
        endScreenCanvas.SetActive(true);
        StartCoroutine(FadeInEndScreen());
    }

    System.Collections.IEnumerator FadeInEndScreen()
    {
        endScreenGroup.alpha = 0;
        while (endScreenGroup.alpha < 1f)
        {
            endScreenGroup.alpha += Time.deltaTime;
            yield return null;
        }
    }
}
