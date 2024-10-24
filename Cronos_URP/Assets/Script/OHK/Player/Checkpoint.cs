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
    public int priority;
    public float healTP;

    [Header("Event")]
    public UnityEvent OnActive;

    [SceneName]
    private string _sceneName;
    private bool _isActive;

    private readonly string purpose = "_checkpoint";
    private readonly string tp = "checkPoint_TP";

    private AbilityTree _abilityTree;


    // -----

    public void LoadData()
    {
        Player.Instance.TP = PlayerPrefs.GetFloat(tp);

        _abilityTree.LoadData(purpose);

        if (_sceneName != SceneManager.GetActiveScene().name)
        {
            SceneController.TransitionToScene(_sceneName);
        }
    }

    // -----

    private void Awake()
    {
        _sceneName = SceneManager.GetActiveScene().name;

        _abilityTree = FindObjectOfType<AbilityTree>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive == true)
        {
            return;
        }

        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        OnActive?.Invoke();

        _isActive = true;

        player.TP += healTP;

        // 데이터 저장
        SaveData();

    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        if (_isActive == false)
        {
            player.isDecreaseTP = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue * 0.75f;
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    // -----

    private void SaveData()
    {
        SaveLoadManager.Instance.currentCheckpoint = this;

        PlayerPrefs.SetFloat(tp, Player.Instance.TP);

        _abilityTree.SaveData(purpose);
    }
}
