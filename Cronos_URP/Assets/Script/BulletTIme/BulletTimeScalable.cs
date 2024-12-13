using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class BulletTimeScalable : MonoBehaviour
{
    [SerializeField]
    private bool active = true;

    protected Animator _animator;
    protected NavMeshAgent _navMeshAgent;

    // -----

    public void SetActive(bool set)
    {
        active = set;
    }

    public float GetDeltaTime()
    {
        if (active == true)
        {
            return Time.deltaTime * BulletTime.Instance.GetCurrentSpeed();
        }

        return Time.deltaTime;
    }

    // -----

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }


	private float _timer;

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
