using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class VRMenuController : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject menuCanvas;
    public TMP_Text statsText;
    public Button spawnButton;

    [Header("Hand + Input")]
    public Transform leftHand;
    public InputActionProperty toggleMenuAction;

    [Header("Trash Spawning")]
    public GameObject[] trashPrefabs;
    public Transform spawnArea;

    void Start()
    {
        spawnButton.onClick.AddListener(SpawnTrash);
        menuCanvas.transform.SetParent(leftHand);
        menuCanvas.transform.localPosition = new Vector3(0.15f, 0.15f, 0.1f);
        menuCanvas.transform.localRotation = Quaternion.Euler(0, 0, 0);
        menuCanvas.SetActive(true);
    }

    void Update()
    {
        if (toggleMenuAction.action.WasPressedThisFrame())
        {
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }

        UpdateStats();
    }

    void UpdateStats()
    {
        var tm = TrashManager.Instance;
        statsText.text = $"Total: {tm.total}\nVerre: {tm.dechetvert}\nEmballage: {tm.dechetjaune}\nAliment: {tm.dechetmarron}";
    }

    void SpawnTrash()
    {
        var prefab = trashPrefabs[Random.Range(0, trashPrefabs.Length)];
        Vector3 spawnPos = spawnArea.position + Random.insideUnitSphere * 0.3f;
        GameObject instance = Instantiate(prefab, spawnPos, Quaternion.identity);

        string type = prefab.tag.ToLower(); // Assure-toi que tes prefabs ont un tag : "glass", "packaging", "food"
        TrashManager.Instance.AddTrash(type);
    }
}
