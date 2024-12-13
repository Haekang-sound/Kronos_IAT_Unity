using UnityEngine;

/// <summary>
/// 공격 스택을 관리하는 클래스입니다. 
/// 공격이 발생할 때마다 스택이 증가하며, 주어진 시간 동안 스택을 유지합니다.
/// </summary>
public class AttackStack : MonoBehaviour
{
    public int currentStack;
    public int maxStack;
    public float duration;
    public SimpleDamager damager;

    private float _timer;

    void Awake()
    {
        damager.OnAttack.AddListener(AddAttackStack);
    }

    private void OnDistroy()
    {
        damager.OnAttack.RemoveAllListeners();
    }

    private void Update()
    {
        if (currentStack > 0)
        {
            _timer += Time.unscaledDeltaTime;

            if (_timer < duration)
            {
                _timer = 0f;
                RemoveAttackStack();
            }
        }
    }

    void AddAttackStack()
    {
        currentStack += 1;
        if (currentStack > maxStack)
        {
            currentStack = maxStack;
        }
    }

    void RemoveAttackStack()
    {
        currentStack -= 1;
        if (currentStack < 0)
        {
            currentStack = 0;
        }
    }
}
