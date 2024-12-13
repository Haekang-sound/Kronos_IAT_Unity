using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ������ ���� �� �ε� ����� ����ϴ� Ŭ�����Դϴ�.
/// �� ������, üũ����Ʈ ������, �ɷ� Ʈ�� �����͸� �����ϰ� �ҷ����� ����� �����մϴ�.
/// </summary>
public class SaveLoadManager : MonoBehaviour
{
    /// <summary>
    /// ���� �� �ε��� ������ �����ϴ� �������Դϴ�. 
    /// �� �����Ϳ� üũ����Ʈ �����ͷ� ���е˴ϴ�.
    /// </summary>
    public enum Purpose
    {
        scene,      // �� ������ ���� �� �ε�
        checkpoint  // üũ����Ʈ ������ ���� �� �ε�
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
            if (_instance == null)
            {
                _instance = FindObjectOfType<SaveLoadManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("SaveLoadManager");
                    _instance = go.AddComponent<SaveLoadManager>();
                }
            }
            return _instance;
        }
    }

    protected static SaveLoadManager _instance;

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
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        _abilityTree = FindObjectOfType<AbilityTree>();
    }
}
