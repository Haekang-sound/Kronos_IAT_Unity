using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// ��� Enmy ��ü���� ���������� ������ ������Ʈ �� ������ ������ Ŭ����
/// ��ü�� ����, �׺�޽�, 
/// </summary>
[DefaultExecutionOrder(-1)] // �ٸ� ��ũ��Ʈ���� ���� ����(���� �ֹ� ���� ���� ���� ���� ����)
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    // �ν����� â�� �������� ������
    // OnEnable �̺�Ʈ���� Tag �� ���� Player ��ü�� ã�´�.
    [Header("Player Settings")]
    public GameObject target;

    [Header("Movement Settings ")]
    public bool useAnimatiorSpeed = true;
    public bool interpolateTurning;
    public bool applyAnimationRotation;
    [Range(0f, 100f)]
    public float rotationLerpSpeed;

    public Animator animator { get { return _animator; } }
    public Vector3 externalForce { get { return _externalForce; } }
    public NavMeshAgent navmeshAgent { get { return _navMeshAgent; } }
    public bool followNavmeshAgent { get { return _followNavmeshAgent; } }
    public bool grounded { get { return _grounded; } }

    protected NavMeshAgent _navMeshAgent;
    protected bool _followNavmeshAgent;
    protected Animator _animator;
    protected bool _underExternalForce;
    protected bool _externalForceAddGravity = true;
    protected Vector3 _externalForce;
    protected bool _grounded;
    protected Rigidbody _rigidbody;

    const float _groundedRayDistance = .8f;

    void OnEnable()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

        _navMeshAgent.updatePosition = false;

        _rigidbody = GetComponentInChildren<Rigidbody>();

        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        _rigidbody.drag = 10f;
        _rigidbody.angularDrag = 1000f;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        _followNavmeshAgent = true;
    }

    private void FixedUpdate()
    {
        CheckGrounded();

        if (_underExternalForce)
        {
            ForceMovement();
        }
    }

    public void SetRotationLerpSeedFast()
    {
        rotationLerpSpeed = 14f;
    }

    public void SetRotationLerpSeedNormal()
    {
        rotationLerpSpeed = 3f;
    }

    public void SetRotationLerpSeedSlow()
    {
        rotationLerpSpeed = 1f;
    }

    public void SetRotationLerpSeedZero()
    {
        rotationLerpSpeed = 0f;
    }

    public void SetForwardToTarget(Vector3 targetPostion)
    {
        Vector3 direction = targetPostion - transform.position;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, rotationLerpSpeed * Time.deltaTime);
    }

    // ���� ���� �ִ��� �˻��Ѵ�.
    void CheckGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * _groundedRayDistance * 0.5f, -Vector3.up);
        _grounded = Physics.Raycast(ray, out hit, _groundedRayDistance, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// �ܺο� ���� �����¿� ���� ����� �Ѵ�.
    /// </summary>
    void ForceMovement()
    {
        if (_externalForceAddGravity)
        {
            _externalForce += Physics.gravity * Time.deltaTime;
        }

        RaycastHit hit;
        Vector3 movement = _externalForce * Time.deltaTime;
        if (!_rigidbody.SweepTest(movement.normalized, out hit, movement.sqrMagnitude))
        {
            _rigidbody.MovePosition(_rigidbody.position + movement);
        }

        _navMeshAgent.Warp(_rigidbody.position);
    }
    private void OnAnimatorMove()
    {
        // �ܺ� �з��� ���� ��� �ִϸ��̼��� ����Ǿ�� �ȵȴ�.
        if (_underExternalForce) return;

        // ���� �����ӿ��� �̵��� �Ÿ��� �ð� ������ ���� �ӵ��� �����Ѵ�.
        if (_followNavmeshAgent)
        {
            if (useAnimatiorSpeed)
            {
                _navMeshAgent.speed = (_animator.deltaPosition / Time.deltaTime).magnitude;
            }
            transform.position = _navMeshAgent.nextPosition;
        }
        else
        {
            // �浹 �˻� �� �̵�. ���� �浹�� �߻��Ѵٸ� rigidbody�� ��ġ�� ������ ����.
            RaycastHit hit;
            if (!_rigidbody.SweepTest(_animator.deltaPosition.normalized, out hit,
                _animator.deltaPosition.sqrMagnitude))
            {
                _rigidbody.MovePosition(_rigidbody.position + _animator.deltaPosition);
            }
        }

        if (applyAnimationRotation)
        {
            // ���� ��ü�� ���� ������ �ִϸ��̼� ȸ���� �°� ����
            transform.forward = _animator.deltaRotation * transform.forward;
        }
    }

    public void SetFollowNavmeshAgent(bool follow)
    {
        if (!follow && _navMeshAgent.enabled)
        {
            _navMeshAgent.ResetPath();
        }
        else if (follow && !_navMeshAgent.enabled)
        {
            _navMeshAgent.Warp(transform.position);
        }

        _followNavmeshAgent = follow;
        _navMeshAgent.enabled = follow;
    }

    public void UseNavemeshAgentRotation(bool use)
    {
        _navMeshAgent.updateRotation = use;
    }

    public void SetNavemeshAgentSpeed(float speed)
    {
        _navMeshAgent.speed = speed;
    }

    public float GetNavemeshAgentSpeed()
    {
        return _navMeshAgent.speed;
    }

    public void AddForce(Vector3 force, bool useGravity = true)
    {
        if (_navMeshAgent.enabled)
        {
            _navMeshAgent.ResetPath();
        }

        _externalForce = force;
        _navMeshAgent.enabled = false;
        _underExternalForce = true;
        _externalForceAddGravity = useGravity;
    }

    public void ClearForce()
    {
        _underExternalForce = false;
        _navMeshAgent.enabled = true;
    }

    public void SetForward(Vector3 forward)
    {
        Quaternion targetRotation = Quaternion.LookRotation(forward);

        if (interpolateTurning)
        {
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                _navMeshAgent.angularSpeed * Time.deltaTime);
        }

        transform.rotation = targetRotation;
    }

    public bool SetTarget(Vector3 position)
    {
        if (_followNavmeshAgent == false)
        {
            return false;
        }

        return _navMeshAgent.SetDestination(position);
    }
}
