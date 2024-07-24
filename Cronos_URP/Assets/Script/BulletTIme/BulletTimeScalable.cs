using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class BulletTimeScalable : MonoBehaviour
{
    public bool active = true;

    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        if (active == false)
        {
            _animator.speed = 1f;
        }
    }

    void OnAnimatorMove()
    {
        if (active == true)
        {
            _animator.speed = BulletTime.Instance.GetCurrentSpeed();
            _navMeshAgent.speed *= BulletTime.Instance.GetCurrentSpeed();
        }
    }
}
