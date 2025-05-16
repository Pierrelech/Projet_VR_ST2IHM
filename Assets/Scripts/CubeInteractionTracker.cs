/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class CubeInteractionTracker : MonoBehaviour
{
    public XRGrabInteractable interactable;

    [Header("UI (optionnel)")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI attemptsText;

    [HideInInspector] public bool isPlacedCorrectly = false;

    private float timer = 0f;
    private int attemptCount = 0;
    private bool isBeingHeld = false;
    private bool trackingEnabled = false;

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnGrab);
        interactable.selectExited.RemoveListener(OnRelease);
    }

    void Update()
    {
        if (isBeingHeld && trackingEnabled)
        {
            timer += Time.deltaTime;
            UpdateUI();
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (!isPlacedCorrectly)
        {
            //  Ne pas reset ici ? juste activer le chrono
            isBeingHeld = true;
            trackingEnabled = true;
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isBeingHeld = false;

        if (!isPlacedCorrectly)
        {
            attemptCount++; // compte l'essai
            UpdateUI();
            //  NE PAS reset le timer
        }
    }

    public void MarkAsPlaced()
    {
        if (!isPlacedCorrectly)
        {
            isPlacedCorrectly = true;
            trackingEnabled = false;
            UpdateUI();
        }
    }

    public void ResetTracker()
    {
        isPlacedCorrectly = false;
        isBeingHeld = false;
        trackingEnabled = false;
        timer = 0f;
        attemptCount = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (timeText != null)
            timeText.text = $"Temps : {timer:F2}s";

        if (attemptsText != null)
            attemptsText.text = $"Essais : {attemptCount}";
    }

    public float GetFinalTime() => timer;
    public int GetAttemptCount() => attemptCount;
}
*/ 

// Version 2

/*using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class CubeInteractionTracker : MonoBehaviour
{
    public XRGrabInteractable interactable;

    [Header("UI (optionnel)")]
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI debugText;  // Affichage debug

    [HideInInspector] public bool isPlacedCorrectly = false;

    [HideInInspector] public bool canBePlaced = false;

    private float timer = 0f;
    private int attemptCount = 0;
    private bool isBeingHeld = false;
    private bool trackingEnabled = false;

    private bool hasBeenGrabbedThisCycle = false;

    void Start()
    {
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnGrab);
            interactable.selectExited.AddListener(OnRelease);
        }
    }

    void OnEnable()
    {
        interactable.selectEntered.AddListener(OnGrab);
        interactable.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnGrab);
        interactable.selectExited.RemoveListener(OnRelease);
    }

    void Update()
    {
        if (trackingEnabled && !isPlacedCorrectly)
        {
            timer += Time.deltaTime;
            UpdateUI();
        }
        else
        {
            UpdateUI(); // pour garder l'affichage en temps réel
        }
    }    

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
        if (!isPlacedCorrectly)
        {
            isPlacedCorrectly = true;
            trackingEnabled = false;
        }
    }

    public bool IsBeingHeld()
    {
        return isBeingHeld;
    }

    public void ResetTracker()
    {
        isPlacedCorrectly = false;
        isBeingHeld = false;
        trackingEnabled = false;
        timer = 0f;
        attemptCount = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (timeText != null)
            timeText.text = $"Temps : {timer:F2}s";

        if (attemptsText != null)
            attemptsText.text = $"Essais : {attemptCount}";

        if (debugText != null)
        {
            debugText.text = $"[DEBUG]\n" +
                             $"trackingEnabled: {trackingEnabled}\n" +
                             $"isPlacedCorrectly: {isPlacedCorrectly}\n" +
                             $"isBeingHeld: {isBeingHeld}\n" +
                             $"Temps: {timer:F2}s\n" +
                             $"Essais: {attemptCount}";
        }
    }

    public float GetFinalTime() => timer;
    public int GetAttemptCount() => attemptCount;
}
*/

// Version 3

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

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
        // Si l’interactable n'est pas attribué, le trouver automatiquement
        if (interactable == null)
        {
            interactable = GetComponent<XRGrabInteractable>();
            if (interactable == null)
            {
                Debug.LogError($" Aucun XRGrabInteractable trouvé sur {gameObject.name}");
                enabled = false;
                return;
            }
        }

        // Abonnement aux événements
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
            Debug.Log($" Grab commencé sur {gameObject.name}");
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
            Debug.Log($" Grab relâché sur {gameObject.name} — Tentatives : {attemptCount}");
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
        Debug.Log($" Cube bien placé : {gameObject.name} — Temps : {timer:F2}s — Tentatives : {attemptCount}");
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
        Debug.Log($" Tracker réinitialisé : {gameObject.name}");
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
                             " À déposer";
    }

    public bool IsBeingHeld()
    {
        return isBeingHeld;
    }
}



