using System.IO;
using UnityEngine;

public class UserDataExport : MonoBehaviour
{
    public string fileName = "UserData.csv";

    public void ExportData(float totalTime, int correct, int errors)
    {
        string folderPath = Path.Combine(Application.dataPath, "UserResult");
        string filePath = Path.Combine(folderPath, fileName);

        // Crée le dossier s'il n'existe pas
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Vérifie si le fichier existe
        bool fileExists = File.Exists(filePath);

        using (StreamWriter writer = new StreamWriter(filePath, true)) // true = append
        {
            if (!fileExists)
            {
                writer.WriteLine("Temps Total (s),Score,Erreurs");
            }

            string line = $"{totalTime:F2},{correct},{errors}";
            writer.WriteLine(line);
        }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh(); // Rafraîchir l'AssetDatabase pour voir le fichier dans l'éditeur
#endif

        Debug.Log("Fichier CSV écrit à : " + filePath);
    }
}
