using UnityEngine;

public class TrashManager : MonoBehaviour
{
    public static TrashManager Instance;

    public int total = 0;
    public int DechetVert = 0;
    public int DechetJaune = 0;
    public int DechetMarron = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void AddTrash(string type)
    {
        total++;
        switch (type)
        {
            case "DechetVert": DechetVert++; break;
            case "DechetJaune": DechetJaune++; break;
            case "DechetMarron": DechetMarron++; break;
        }
    }
}
