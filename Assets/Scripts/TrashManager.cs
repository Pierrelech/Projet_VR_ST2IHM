using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public static TrashManager Instance;

    public int total = 0;
    public int dechetvert = 0;
    public int dechetjaune = 0;
    public int dechetmarron = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddTrash(string type)
    {
        total++;
        switch (type)
        {
            case "glass": dechetvert++; break;
            case "packaging": dechetjaune++; break;
            case "food": dechetmarron++; break;
        }
    }
}
