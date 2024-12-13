using Message;
using UnityEngine;

/// <summary>
/// 디버깅을 위한 더미캐릭터 객체들이 공동으로 사용하는 이동, 물리 처리, 외부 힘 적용 등의 기능을 정의한 클래스입니다.
/// NavMeshAgent를 활용한 경로 탐색, 애니메이션 기반 이동, 외부 물리력 처리를 제공합니다.
/// </summary>
[DefaultExecutionOrder(-1)] // 다른 스크립트보다 먼저 실행(실행 주문 값이 낮을 수록 먼저 실행)
public class DummyController : MonoBehaviour, IMessageReceiver
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private HitShake _hitShake;
    private KnockBack _knockBack;
    private Damageable _damageable;
    private BulletTimeScalable _bulletTimeScalable;

    public static readonly int hashDamage = Animator.StringToHash("damage");

    void Awake() 
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _hitShake = GetComponent<HitShake>();
        _knockBack = GetComponent<KnockBack>();
        _damageable = GetComponent<Damageable>();
        _bulletTimeScalable = GetComponent<BulletTimeScalable>();
    }

    void OnEnable()
    {
        _rigidbody.drag = 10f;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        _damageable.onDamageMessageReceivers.Add(this);
    }

    void OnDisable()
    {
        _damageable.onDamageMessageReceivers.Remove(this);
    }

    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        var dmgMsg = (Damageable.DamageMessage)msg;

        switch (type)
        {
            case MessageType.DAMAGED:
                Damaged(dmgMsg);
                break;
            case MessageType.DEAD:
                break;
            case MessageType.RESPAWN:
                break;
            default:
                return;
        }
    }

    private void Damaged(Damageable.DamageMessage msg)
    {
        UnuseBulletTimeScale();
        _animator.SetTrigger(hashDamage);

        _hitShake?.Begin();

        _knockBack?.Begin(msg.damageSource);
    }

    internal void UseBulletTimeScale()
    {
        _bulletTimeScalable.SetActive(true);
    }

    internal void UnuseBulletTimeScale()
    {
        _bulletTimeScalable.SetActive(false);
    }
}
