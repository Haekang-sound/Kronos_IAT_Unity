using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Player의 기본적인 상태이자
/// 이동 상태를 정의한 클래스
///
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    private const float _animationDampTime = 0.1f;
    private float _moveSpeed = 0.5f;

    public float targetSpeed = 0.5f;
    
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (Player.Instance.groggyStack != null)
        {
            Player.Instance.groggyStack.ResetStack();
        }

        _stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false);
        _stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
        _stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
        _stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.CombAttack);
        _stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.ParryAttack);
        _stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.CPBoomb);

        _stateMachine.InputReader.onFlashSlashStart += FlashSlash;
        _stateMachine.InputReader.onRunStart += RushAttack;

        _stateMachine.InputReader.onLAttackStart += Attack;
        _stateMachine.InputReader.onRAttackStart += Gurad;
        _stateMachine.InputReader.onJumpStart += Dodge;

        _stateMachine.InputReader.onLAttackCanceled += ReleaseAttack;
        _stateMachine.InputReader.onRAttackCanceled += ReleaseGuard;
    }

    public override void Tick()
    {
        _moveSpeed = 1f;

        if (_stateMachine.velocity.magnitude != 0f)
        {
            //stateMachine.Animator.SetBool("isMove", true);
        }
        else
        {
            _stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsMove, false);
        }

        // 애니메이터 movespeed의 파라메터의 값을 정한다.
        // 락온 상태일때 && 달리기가 아닐때
        if (_stateMachine.Player.IsLockOn && _moveSpeed < 0.6f)
        {
            // moveSpeed에 y값을곱해서 전방이동인지 후방이동인지 확인한다.
            _stateMachine.Animator.SetFloat(PlayerHashSet.Instance.MoveSpeed,
                    (_moveSpeed * _stateMachine.InputReader.moveComposite.y), _animationDampTime, Time.deltaTime);
        }
        else
        {
            _stateMachine.Animator.SetFloat(PlayerHashSet.Instance.MoveSpeed, _stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? _moveSpeed : 0f, _animationDampTime, Time.deltaTime);
        }

        if (_stateMachine.Player.IsLockOn && _moveSpeed < 0.7f)
        {
            float side = 0f;
            side = _stateMachine.InputReader.moveComposite.x * 0.75f;
            _stateMachine.Animator.SetFloat(PlayerHashSet.Instance.SideWalk, side, _animationDampTime, Time.deltaTime);
        }
        else
        {
            _stateMachine.Animator.SetFloat(PlayerHashSet.Instance.MoveX, _stateMachine.InputReader.moveComposite.x, _animationDampTime, Time.deltaTime);
            _stateMachine.Animator.SetFloat(PlayerHashSet.Instance.MoveY, _stateMachine.InputReader.moveComposite.y, _animationDampTime, Time.deltaTime);
            _stateMachine.Animator.SetFloat(PlayerHashSet.Instance.SideWalk, _stateMachine.InputReader.moveComposite.x, _animationDampTime, Time.deltaTime);
        }

        CalculateMoveDirection();
    }

    public override void FixedTick()
    {
        if (_stateMachine.Player.IsLockOn)
        {
            if (_moveSpeed > 0.5f)
            {
                FaceMoveDirection();
            }
        }
        else
        {
            FaceMoveDirection();
        }

        Move();
    }

    public override void LateTick() { }

    public override void Exit()
    {
        _stateMachine.InputReader.onFlashSlashStart -= FlashSlash;
        _stateMachine.InputReader.onRunStart -= RushAttack;

        _stateMachine.InputReader.onLAttackStart -= Attack;
        _stateMachine.InputReader.onLAttackCanceled -= ReleaseAttack;
        _stateMachine.InputReader.onRAttackStart -= Gurad;
        _stateMachine.InputReader.onJumpStart -= Dodge;

        _stateMachine.InputReader.onRAttackCanceled -= ReleaseGuard;
        _stateMachine.Animator.speed = 1f;
    }


    private void ReleaseAttack() { _stateMachine.InputReader.clickCondition = false; }
    private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsGuard, true); }
    public void ReleaseGuard() { _stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsGuard, false); }

    // 값 변화를 부드럽게 주자
    IEnumerator SmoothChangeSpeed()
    {
        float startSpeed = _moveSpeed;
        float elapsedTime = 0.0f;

        while (elapsedTime < 0.1f)
        {
            _moveSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _moveSpeed = targetSpeed; // Ensure it reaches the target value at the end
    }

    private void Attack()
    {
        _stateMachine.autoTargetting.AutoTargeting();
        if (!_stateMachine.Player.isBuff)
        {
            _stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Attack);
            _stateMachine.Animator.SetBool(PlayerHashSet.Instance.IsMove, true);
            Player.Instance.isBuff = false;
        }
        else
        {
            _stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
            _stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.CombAttack);
        }

    }

    private void Dodge()
    {
        if (_stateMachine.InputReader.moveComposite.magnitude != 0f && !CoolTimeCounter.Instance.IsDodgeUsed)
        {
            CoolTimeCounter.Instance.IsDodgeUsed = true;
            if (_stateMachine.InputReader.moveComposite.magnitude != 0)
            {
                _stateMachine.Rigidbody.rotation = Quaternion.LookRotation(_stateMachine.velocity);
            }
            _stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Dodge);
        }
    }

    // 일섬
    public void FlashSlash()
    {
        if (_stateMachine.Animator.GetBool(PlayerHashSet.Instance.IsFlashSlash) && Player.Instance.CP >= 20f)
        {
            _stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.FlashSlash);
            Player.Instance.CP -= 25f;
        }
    }

    // 시간베기
    public void TimeSlash()
    {
        if (_stateMachine.Animator.GetBool(PlayerHashSet.Instance.IsTimeSlash))
        {
            if (!_stateMachine.autoTargetting.FindTarget()) return;
            _stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.TimeSlashReady);
        }
    }

    private void RushAttack()
    {
        if (_stateMachine.Animator.GetBool(PlayerHashSet.Instance.IsRushAttack) && !CoolTimeCounter.Instance.IsRushAttackUsed)
        {
            _stateMachine.autoTargetting.enabled = true;
            PlayerStateMachine.GetInstance().autoTargetting.sphere.enabled = true;

            CoolTimeCounter.Instance.IsRushAttackUsed = true;
            _stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.RushAttack);
        }
    }
}




