using UnityEngine;

/// <summary>
/// 레벨의 문을 열 수 있는 트리거를 관리하는 클래스입니다.
/// 지정된 `Damageable` 객체가 사망하면 `timeline` 오브젝트를 활성화합니다.
/// </summary>
public class LevelDoorDamagerbleTrigger : MonoBehaviour
{
    public Damageable damageable;
    public GameObject timeline;

    void Awake()
    {
        if (timeline != null)
        {
            damageable?.OnDeath.AddListener(() => timeline.SetActive(true));
        }
    }
}
