using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<SaveLoadManager>();
            if (instance != null)
                return instance;

            Create();
            return instance;
        }
    }

    protected static SaveLoadManager instance;

    public static SaveLoadManager Create()
    {
        GameObject dataManagerGameObject = new GameObject("PersistentDataManager");
        DontDestroyOnLoad(dataManagerGameObject);
        instance = dataManagerGameObject.AddComponent<SaveLoadManager>();
        return instance;
    }

    // =====

    // -----

    public static void SaveAllData()
    {
        Player.Instance.Save();

        var at = GameObject.Find("AbilityUnlock").GetComponent<AbilityTree>();
        if (at != null)
        {
            at.SaveData();
        }
    }

    public static void LoadAllData()
    {
        Player.Instance.Load();
    }

    public static void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }
}
