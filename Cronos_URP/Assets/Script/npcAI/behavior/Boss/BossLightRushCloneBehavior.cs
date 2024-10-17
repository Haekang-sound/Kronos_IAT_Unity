using System.Collections;
using UnityEngine;

public class BossLightRushCloneBehavior : MonoBehaviour
{
    public float activeTime;
    public float lifeTime = 3f;
    public GameObject damager;
    public GameObject target;

    private bool _aim;
    private float _rotationSpeed = 20f;

    private Animator _animator;
    private Rigidbody _rigidbody;

    private float elapsedTime;

    private void Awake()
    {
        target = Player.Instance.gameObject;
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        SceneLinkedSMB<BossLightRushCloneBehavior>.Initialise(_animator, this);
    }

    private void Start()
    {
        StartCoroutine(RushAfterSeconds());
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime * BulletTime.Instance.GetCurrentSpeed();
        if (elapsedTime > lifeTime + activeTime)
        {
            Destroy(gameObject);
        }

        if (_aim)
        {
            LookAtTarget();
        }
    }

    private void OnAnimatorMove()
    {
        //_rigidbody.velocity = _animator.velocity;
        RaycastHit hit;
        if (!_rigidbody.SweepTest(_animator.deltaPosition.normalized, out hit,
            _animator.deltaPosition.sqrMagnitude))
        {
            _rigidbody.MovePosition(_rigidbody.position + _animator.deltaPosition);
        }

        transform.forward = _animator.deltaRotation * transform.forward;
    }

    public void BeginAiming()
    {
        _aim = true;
    }

    public void StopAiming()
    {
        _aim = false;
    }

    public void LookAtTarget()
    {
        if (target == null) return;

        // 바라보는 방향 설정
        var lookPosition = target.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _rotationSpeed);
    }

    private IEnumerator RushAfterSeconds()
    {
        yield return new WaitForSeconds(activeTime);
        Rush();
        OffAim();
    }

    public void BeginAttack()
    {
        damager.SetActive(true);
    }
    public void StopAttack()
    {
        damager.SetActive(false);
    }

    private void Rush()
    {
        if (_animator != null)
        {
            _animator.SetTrigger("rush");
        }
    }

    // 이펙트 때문에 만들어야지
    // by MIC
    public void ChargeAim()
    {
        gameObject.GetComponent<BossChargeAimer>().DoAim();
    }

    // 1 + 1
    public void OffAim()
    {
        gameObject.GetComponent<BossChargeAimer>().OffAim();
    }
}
