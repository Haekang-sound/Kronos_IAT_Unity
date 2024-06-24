using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

/// <summary>
/// 모든 Enmy 객체들이 공동적으로 가지는 컴포넌트 및 변수를 정의한 클래스
/// 객체의 물리, 네브메시, 
/// </summary>
[DefaultExecutionOrder(-1)] // 다른 스크립트보다 먼저 실행(실행 주문 값이 낮을 수록 먼저 실행)
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour
{
    // 인스펙터 창에 지정하지 않으면
    // OnEnable 이벤트에서 Tag 를 통해 Player 객체를 찾는다.
    [Header("Player Settings")]
    public GameObject player;

    [Header("Movement Settings ")]
    public bool interpolateTurning = false;
    public bool applyAnimationRotation = false;
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

    protected bool _bulletTimeScaled = true;

    const float _groundedRayDistance = .8f;

    void OnEnable()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
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

        _rigidbody.isKinematic = true;
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
        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, lookRotation, rotationLerpSpeed * Time.deltaTime * BulletTime.Instance.GetCurrentSpeed());
    }

    // 지면 위에 있는지 검사한다.
    void CheckGrounded()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position + Vector3.up * _groundedRayDistance * 0.5f, -Vector3.up);
        _grounded = Physics.Raycast(ray, out hit, _groundedRayDistance, Physics.AllLayers,
            QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// 외부에 가한 물리력에 대해 계산을 한다.
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
        // 외부 압력이 있을 경우 애니메이션이 재생되어서는 안된다.
        if (_underExternalForce)
            return;

        if (_bulletTimeScaled != false)
        {
            _animator.speed = BulletTime.Instance.GetCurrentSpeed();
        }
        else
        {
            _animator.speed = 0.8f;
        }

        // 현재 프레임에서 이동한 거리와 시간 단위로 값을 속도로 지정한다.
        if (_followNavmeshAgent)
        {
            _navMeshAgent.speed = (_animator.deltaPosition / Time.deltaTime).magnitude * BulletTime.Instance.GetCurrentSpeed();

            /// test용. 
            /// deltaPosition 값이 없는 보스 걷기 모션을 위한 임시방변.
            /// TODO - 네브메시에이전트 스피드를 외부에서 설정할 수 있게 해야겠다.
            if (_animator.deltaPosition.magnitude <= 0.0f)
            {
                _navMeshAgent.speed = 3f * BulletTime.Instance.GetCurrentSpeed(); ;
            }

            transform.position = _navMeshAgent.nextPosition;
        }
        else
        {
            // 충돌 검사 후 이동. 만일 충돌이 발생한다면 rigidbody의 위치는 변하지 않음.
            RaycastHit hit;
            if (!_rigidbody.SweepTest(_animator.deltaPosition.normalized, out hit,
                _animator.deltaPosition.sqrMagnitude))
            {
                _rigidbody.MovePosition(_rigidbody.position + _animator.deltaPosition);
            }
        }

        if (applyAnimationRotation)
        {
            // 현재 객체의 전방 방향을 애니메이션 회전에 맞게 조정
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
        return _navMeshAgent.SetDestination(position);
    }

    public void SetBulletTime(bool useBulletTimeScale)
    {
        _bulletTimeScaled = useBulletTimeScale;
    }
}
