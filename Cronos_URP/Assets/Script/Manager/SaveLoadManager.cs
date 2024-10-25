using System.ComponentModel;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    public enum Purpose
    {
        _scene,
        _checkpoint
    }

    [SerializeField]
    private CheckpointData _currentCheckpoint;
    public CheckpointData CurrentCheckpoint {
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
        _lastSavedScenename = SceneManager.GetActiveScene().name;

        PlayerPrefs.SetFloat(_lastSavedScenename + "_TP", Player.Instance.TP);
        PlayerPrefs.SetFloat(_lastSavedScenename + "_CP", Player.Instance.CP);

        _abilityTree.SaveData(SaveLoadManager.Purpose._scene.ToString());
    }

    public void LoadSceneData()
    {
        if (PlayerPrefs.HasKey(_lastSavedScenename + "_TP"))
        {
            Player.Instance.TP = PlayerPrefs.GetFloat(_lastSavedScenename + "_TP");
            Player.Instance.CP = PlayerPrefs.GetFloat(_lastSavedScenename + "_CP");

            _abilityTree.LoadData(SaveLoadManager.Purpose._scene.ToString());
        }
    }

    public void LoadCheckpointData()
    {
        if(_currentCheckpoint == null)
        {
            Debug.Log("저징된 체크포인트가 없는데스. 씬 내 체크포인트를 다시 확인 할 것");
            return;
        }

        _currentCheckpoint.LoadData();
    }

    // -----

    private void Awake()
    {
        _abilityTree = FindObjectOfType<AbilityTree>();
    }
}
