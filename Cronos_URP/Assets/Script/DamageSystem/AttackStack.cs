using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
