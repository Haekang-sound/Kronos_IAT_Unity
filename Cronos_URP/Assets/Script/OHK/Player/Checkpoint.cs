using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


/// <summary>
/// üũ����Ʈ�� 
/// - �� ���� �ߵ��մϴ�
/// - 
/// </summary>
[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    private bool _drawGizmo = true;

    public int priority;
    public float healTP;
    private CheckpointData _data;

    [Header("Event")]
    public UnityEvent OnActive;

    private bool _isActive;

    private AbilityTree _abilityTree;

    private readonly string k_purpose = "_checkpoint";

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isActive == true)
        {
            return;
        }

        Player player = other.GetComponent<Player>();

        if (player != null)
        {

            OnActive?.Invoke();

            _isActive = true;

            player.TP += healTP;

            // ������ ����
            _data.SaveData();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        if (_isActive == true)
        {
            player.isDecreaseTP = true;
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
