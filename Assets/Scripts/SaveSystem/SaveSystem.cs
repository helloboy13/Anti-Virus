using UnityEngine;
using System.IO;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);

        Debug.Log("Game Saved!");
        Debug.Log(path);
    }

    public static SaveData Load()
    {
        if (!File.Exists(path))
        {
            Debug.Log("No Save File Found");
            return null;
        }

        string json = File.ReadAllText(path);

        return JsonUtility.FromJson<SaveData>(json);
    }

    public static bool SaveExists()
    {
        return File.Exists(path);
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
}