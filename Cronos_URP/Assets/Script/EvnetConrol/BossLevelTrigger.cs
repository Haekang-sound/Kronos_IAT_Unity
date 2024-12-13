using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ���� �̺�Ʈ ���� �� ���� �ݶ��̴����� �����ϴ� Ŭ����.
/// </summary>
public class BossLevelTrigger : MonoBehaviour
{
    public UnityEvent onEvent;

    private void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != Player.Instance.gameObject) return;

        onEvent?.Invoke();

        gameObject.SetActive(false);
    }
}
