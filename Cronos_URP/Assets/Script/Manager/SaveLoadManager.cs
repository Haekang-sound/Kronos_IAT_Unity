using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임의 저장 및 로드 기능을 담당하는 클래스입니다.
/// 씬 데이터, 체크포인트 데이터, 능력 트리 데이터를 저장하고 불러오는 기능을 제공합니다.
/// </summary>
public class SaveLoadManager : MonoBehaviour
{
    /// <summary>
    /// 저장 및 로드의 목적을 정의하는 열거형입니다. 
    /// 씬 데이터와 체크포인트 데이터로 구분됩니다.
    /// </summary>
    public enum Purpose
    {
        scene,      // 씬 데이터 저장 및 로드
        checkpoint  // 체크포인트 데이터 저장 및 로드
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
            Debug.Log("저징된 체크포인트가 없는데스. 씬 내 체크포인트를 다시 확인 할 것");
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
