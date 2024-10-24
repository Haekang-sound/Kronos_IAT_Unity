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


    private void Awake()
    {
        _sceneName = SceneManager.GetActiveScene().name;
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

        player.SetCheckpoint(this);

        // 플레이어 TP 정보 저장
        Player.Instance.TP += healTP;

        SaveLoadManager.SaveAllData();
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
}
