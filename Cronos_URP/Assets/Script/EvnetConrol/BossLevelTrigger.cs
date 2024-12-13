using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 보스 이벤트 시작 시 맵의 콜라이더들을 제아하는 클리스.
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
