using UnityEngine;
using TMPro;
using System.Collections;



public class ContainerScore : MonoBehaviour
{
    public string acceptedTag; // Le tag des objets acceptés
    public TextMeshPro scoreText;              // Référence au texte TMP
    public Renderer conteneurRenderer;
    public Color correctColor = Color.green;
    public Color incorrectColor = Color.red;
    public Color baseColor;
    public AudioClip successClip;
    public AudioClip errorClip;
    public int error = 0;



    private int score = 0;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedTag))
        {
            score++;
            StartCoroutine(FlashColor(correctColor));
            audioSource.PlayOneShot(successClip);
            Debug.Log("✅ Son joué : " + successClip.name);


            Debug.Log("Déchet accepté ! Score : " + score);

            // Optionnel : détruire l'objet ou le désactiver

        }
        else
        {
            score--;
            StartCoroutine(FlashColor(incorrectColor));
            audioSource.PlayOneShot(errorClip);
            error++;


        }

        if (other.CompareTag("DechetJaune") || other.CompareTag("DechetVert") || other.CompareTag("DechetMarron"))
        {
            UpdateScoreDisplay();
            Destroy(other.gameObject);
        }

    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score;
        }
    }
    public int GetScore() { return score; }
    public int GetErrors() { return error; }
    private IEnumerator FlashColor(Color color)
    {
        conteneurRenderer.material.color = color;
        yield return new WaitForSeconds(2f);
        conteneurRenderer.material.color = baseColor;
    }
}





