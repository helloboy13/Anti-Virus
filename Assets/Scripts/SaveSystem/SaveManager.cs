using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private bool pendingLoad = false;
    private SaveData loadedData;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void SaveGame()
    {
        GameManager gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager not found!");
            return;
        }

        SaveData data = new SaveData();

        gameManager.Save(data);

        SaveSystem.Save(data);
    }

    public void LoadGame()
    {
        loadedData = SaveSystem.Load();

        if (loadedData == null)
            return;

        pendingLoad = true;
    }

    public bool HasSave()
    {
        return SaveSystem.SaveExists();
    }

    public void DeleteSave()
    {
        SaveSystem.DeleteSave();
    }
    

    public bool HasPendingLoad()
    {
        return pendingLoad;
    }

    public SaveData GetLoadedData()
    {
        pendingLoad = false;
        return loadedData;
    }
}