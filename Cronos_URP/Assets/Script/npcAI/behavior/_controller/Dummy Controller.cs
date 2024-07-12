using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)] // �ٸ� ��ũ��Ʈ���� ���� ����(���� �ֹ� ���� ���� ���� ���� ����)
public class DummyController : MonoBehaviour, IMessageReceiver
{
    private Animator _animator;
    private Rigidbody _rigidbody;
    private HitShake _hitShake;
    private Damageable _damageable;

    public static readonly int hashDamage = Animator.StringToHash("damage");

    void Awake() 
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _hitShake = GetComponent<HitShake>();
        _damageable = GetComponent<Damageable>();
    }

    void OnEnable()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;

        _damageable.onDamageMessageReceivers.Add(this);
    }

    void OnDisable()
    {
        _damageable.onDamageMessageReceivers.Remove(this);
    }

    public void OnReceiveMessage(MessageType type, object sender, object msg)
    {
        switch (type)
        {
            case MessageType.DAMAGED:
                Damaged();
                break;
            case MessageType.DEAD:
                break;
            case MessageType.RESPAWN:
                break;
            default:
                return;
        }
    }

    private void Damaged()
    {
        _animator.SetTrigger(hashDamage);
        _hitShake.Begin();
    }
}
