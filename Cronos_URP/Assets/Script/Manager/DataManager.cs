using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance
    {
        get
        {
            if (instance != null)
                return instance;
            instance = FindObjectOfType<DataManager>();
            if (instance != null)
                return instance;

            Create();
            return instance;
        }
    }

    protected static DataManager instance;


    public static DataManager Create()
    {
        GameObject dataManagerGameObject = new GameObject("PersistentDataManager");
        DontDestroyOnLoad(dataManagerGameObject);
        instance = dataManagerGameObject.AddComponent<DataManager>();
        return instance;
    }

    // =====

    // -----

    public static void SaveAllData()
    {
        Player.Instance.Save();
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
