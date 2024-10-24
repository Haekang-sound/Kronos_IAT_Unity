using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    public Checkpoint currentCheckpoint;

    private readonly string purpose = "_scene";
    private readonly string sceneTp = "scene_TP";
    private readonly string sceneCp = "scene_CP";
    private AbilityTree _abilityTree;

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

    public void SaveSceneData()
    {
        PlayerPrefs.SetFloat(sceneTp, Player.Instance.TP);
        PlayerPrefs.SetFloat(sceneCp, Player.Instance.CP);

        _abilityTree.SaveData(purpose);
    }

    public void LoadSceneData()
    {
        if (PlayerPrefs.HasKey(sceneTp))
        {
            Player.Instance.TP = PlayerPrefs.GetFloat(sceneTp);
            Player.Instance.CP = PlayerPrefs.GetFloat(sceneCp);

            _abilityTree.LoadData(purpose);
        }
    }

    // -----

    private void Awake()
    {
        _abilityTree = FindObjectOfType<AbilityTree>();
    }
}
