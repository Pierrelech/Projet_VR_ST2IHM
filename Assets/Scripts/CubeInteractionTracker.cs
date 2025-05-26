using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;


public class CubeInteractionTracker : MonoBehaviour
{
    [Header("XR Interaction")]
    public XRGrabInteractable interactable;

    [Header("UI Feedback (optionnel)")]
    public TMP_Text timeText;
    public TMP_Text attemptsText;
    public TMP_Text debugText;

    [HideInInspector] public bool isPlacedCorrectly = false;
    public bool canBePlaced = false;

    private float timer = 0f;
    private int attemptCount = 0;
    private bool trackingEnabled = false;
    private bool isBeingHeld = false;

    private bool hasBeenGrabbedThisCycle = false;

    void Start()
    {
        // Si l�interactable n'est pas attribu�, le trouver automatiquement
        if (interactable == null)
        {
            interactable = GetComponent<XRGrabInteractable>();
            if (interactable == null)
            {
                Debug.LogError($" Aucun XRGrabInteractable trouv� sur {gameObject.name}");
                enabled = false;
                return;
            }
        }

        // Abonnement aux �v�nements
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnRelease);

        Debug.Log($"Interaction tracker actif sur : {gameObject.name}");
    }

    void Update()
    {
        if (trackingEnabled && !isPlacedCorrectly)
        {
            timer += Time.deltaTime;
            UpdateUI();
        }
    }

    /*public void OnGrab(SelectEnterEventArgs args)
    {
        if (!isPlacedCorrectly)
        {
            isBeingHeld = true;
            trackingEnabled = true;
            Debug.Log($" Grab commenc� sur {gameObject.name}");
        }
    }*/

    public void OnGrab(SelectEnterEventArgs args)
    {
        if (!isPlacedCorrectly && !hasBeenGrabbedThisCycle)
        {
            isBeingHeld = true;
            trackingEnabled = true;

            attemptCount++;
            hasBeenGrabbedThisCycle = true;

            UpdateUI();
        }
    }

    /*public void OnRelease(SelectExitEventArgs args)
    {
        if (!isPlacedCorrectly)
        {
            isBeingHeld = false;
            attemptCount++;
            Debug.Log($" Grab rel�ch� sur {gameObject.name} � Tentatives : {attemptCount}");
        }
    }*/

    public void OnRelease(SelectExitEventArgs args)
    {
        isBeingHeld = false;

        if (!isPlacedCorrectly)
        {
            //trackingEnabled = false;
            hasBeenGrabbedThisCycle = false; // reset pour le prochain grab
        }
    }

    public void MarkAsPlaced()
    {
        isPlacedCorrectly = true;
        trackingEnabled = false;
        Debug.Log($" Cube bien plac� : {gameObject.name} � Temps : {timer:F2}s � Tentatives : {attemptCount}");
    }

    public void ResetTracker()
    {
        isPlacedCorrectly = false;
        trackingEnabled = false;
        isBeingHeld = false;
        timer = 0f;
        attemptCount = 0;
        canBePlaced = false;
        UpdateUI();
        Debug.Log($" Tracker r�initialis� : {gameObject.name}");
    }

    private void UpdateUI()
    {
        if (timeText != null)
            timeText.text = $"Temps : {timer:F2}s";

        if (attemptsText != null)
            attemptsText.text = $"Essais : {attemptCount}";

        if (debugText != null)
            debugText.text = isPlacedCorrectly ? " OK" :
                             isBeingHeld ? " En cours" :
                             " � d�poser";
    }

    public bool IsBeingHeld()
    {
        return isBeingHeld;
    }
}