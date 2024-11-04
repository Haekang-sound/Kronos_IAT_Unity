using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 체크포인트는 
/// - 한 번만 발동합니다
/// - 
/// </summary>
[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private bool _drawGizmo = true;

    public int id;
    private string key;
    public int priority;
    public float healTP;
    private CheckpointData _data;

    [Header("Event")]
    public UnityEvent OnActive;

    public bool IsActive
    {
        set
        {
            if (value == true)
            {
                OnActive?.Invoke();
            }
            _isActive = value;
        }
        get { return _isActive; }
    }
    private bool _isActive;

    private readonly string k_purpose = "_checkpoint";

    private AbilityTree _abilityTree;
    private int _enemyCount;
    // -----

    private void Awake()
    {
        _data = ScriptableObject.CreateInstance<CheckpointData>();

        _data.priority = priority;
        _data.healTP = healTP;
        _data.sceneName = SceneManager.GetActiveScene().name;
        _data.SpwanPos = transform.position;
        _data.SpwanRot = transform.rotation;

        _abilityTree = FindObjectOfType<AbilityTree>();

        // ---

        key = GetType().Name + "-" + _data.sceneName + "-" + id;
    }

    private void Start()
    {
        IsActive = System.Convert.ToBoolean(PlayerPrefs.GetInt(key));
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt(key, System.Convert.ToInt16(IsActive));
    }

    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            _enemyCount++;
        }

        // ---

        Player player = other.GetComponent<Player>();

        if (player == null) return;

        player.isDecreaseTP = false;

        if (IsActive == true) return;

        IsActive = true;
        player.TP += healTP;

        // 데이터 저장
        _data.SaveData();
    }

    private void OnTriggerExit(Collider other)
    {
        var enemy = other.GetComponent<EnemyController>();

        if (enemy != null)
        {
            _enemyCount--;
        }

        // ---

        Player player = other.GetComponent<Player>();

        if (player == null) return;

        player.isDecreaseTP = true;
    }

    private void OnTriggerStay(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            if(_enemyCount > 0)
            {
                player.isDecreaseTP = true;
            }
            else
            {
                player.isDecreaseTP = false;
            }
        }
    }

    void OnDrawGizmos()
    {
        if (_drawGizmo)
        {
            Gizmos.color = Color.blue * 0.75f;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }

}
