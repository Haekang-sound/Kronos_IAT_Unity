using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public enum Purpose
    {
        scene,
        checkpoint
    }

    [SerializeField]
    private CheckpointData _currentCheckpoint;
    public CheckpointData CurrentCheckpoint
    {
        get { return _currentCheckpoint; }
        set
        {
            if (_currentCheckpoint == null
                || value.priority >= _currentCheckpoint.priority)
            {
                _currentCheckpoint = value;
            }
        }
    }

    private AbilityTree _abilityTree;
    private string _lastSavedScenename;

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
        if (Player.Instance == null)
            return;

        _lastSavedScenename = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetFloat(_lastSavedScenename + "-TP", Player.Instance.TP);
        PlayerPrefs.SetFloat(_lastSavedScenename + "-CP", Player.Instance.CP);

        _abilityTree = FindObjectOfType<AbilityTree>();
        _abilityTree.SaveData(SaveLoadManager.Purpose.scene.ToString());
    }

    public void LoadSceneData()
    {
        if (PlayerPrefs.HasKey(_lastSavedScenename + "-TP"))
        {
            Player.Instance.TP = PlayerPrefs.GetFloat(_lastSavedScenename + "-TP");
            Player.Instance.CP = PlayerPrefs.GetFloat(_lastSavedScenename + "-CP");

            _abilityTree = FindObjectOfType<AbilityTree>();
            _abilityTree.LoadData(SaveLoadManager.Purpose.scene.ToString());
        }
    }

    public void LoadCheckpointData()
    {
        if (_currentCheckpoint == null)
        {
            Debug.Log("��¡�� üũ����Ʈ�� ���µ���. �� �� üũ����Ʈ�� �ٽ� Ȯ�� �� ��");
            return;
        }

        StartCoroutine(_currentCheckpoint.LoadData());
    }

    public void DeleteDataAll()
    {
        PlayerPrefs.DeleteAll();
    }

    // -----

    private void Awake()
    {
        _abilityTree = FindObjectOfType<AbilityTree>();
    }
}
