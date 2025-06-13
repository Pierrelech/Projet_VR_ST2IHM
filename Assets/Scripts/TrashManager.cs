using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public static TrashManager Instance;

    public int total = 0;
    public int glass = 0;
    public int packaging = 0;
    public int food = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddTrash(string type)
    {
        total++;
        switch (type)
        {
            case "glass": glass++; break;
            case "packaging": packaging++; break;
            case "food": food++; break;
        }
    }
}
