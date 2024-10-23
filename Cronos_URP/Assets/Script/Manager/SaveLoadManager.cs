using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    private static AbilityTree _abilityUnlock;

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

    // -----

    public static void SaveAllData()
    {
        Player.Instance.Save();
        _abilityUnlock.SaveData();
    }

    public static void LoadAllData()
    {
        Player.Instance.Load();
        _abilityUnlock.LoadData();
    }

    public static void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
    }

    // -----

    private void Awake()
    {
        _abilityUnlock = GameObject.Find("AbilityUnlock").GetComponent<AbilityTree>();
    }

    // -----
}
