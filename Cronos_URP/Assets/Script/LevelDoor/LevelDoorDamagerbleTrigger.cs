using UnityEngine;

/// <summary>
/// ������ ���� �� �� �ִ� Ʈ���Ÿ� �����ϴ� Ŭ�����Դϴ�.
/// ������ `Damageable` ��ü�� ����ϸ� `timeline` ������Ʈ�� Ȱ��ȭ�մϴ�.
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
