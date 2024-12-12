using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Player�� �⺻���� ��������
/// �̵����� ���¸� ������ Ŭ����
///
/// </summary>
public class PlayerMoveState : PlayerBaseState
{
    private const float AnimationDampTime = 0.1f;

    float moveSpeed = 0.5f;
    public float targetSpeed = 0.5f;
    public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        if (Player.Instance.groggyStack != null)
        {
            Player.Instance.groggyStack.ResetStack();
        }

        stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false);
        stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
        stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
        stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.combAttack);
        stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.ParryAttack);
        stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.CPBoomb);


        stateMachine.InputReader.onFlashSlashStart += FlashSlash;
        stateMachine.InputReader.onRunStart += RushAttack;

        stateMachine.InputReader.onLAttackStart += Attack;
        stateMachine.InputReader.onRAttackStart += Gurad;
        stateMachine.InputReader.onJumpStart += Dodge;

        stateMachine.InputReader.onLAttackCanceled += ReleaseAttack;
        stateMachine.InputReader.onRAttackCanceled += ReleaseGuard;

    }

    // state�� update�� �� �� ����
    public override void Tick()
    {
        // �÷��̾��� cp �� �̵��ӵ��� �ݿ��Ѵ�.
        moveSpeed = 1f;

        if (stateMachine.Velocity.magnitude != 0f)
        {
            //stateMachine.Animator.SetBool("isMove", true);
        }
        else
        {
            stateMachine.Animator.SetBool(PlayerHashSet.Instance.isMove, false);
        }

        // �ִϸ����� movespeed�� �Ķ������ ���� ���Ѵ�.
        // ���� �����϶� && �޸��Ⱑ �ƴҶ�
        if (stateMachine.Player.IsLockOn && moveSpeed < 0.6f)
        {
            // moveSpeed�� y�������ؼ� �����̵����� �Ĺ��̵����� Ȯ���Ѵ�.
            stateMachine.Animator.SetFloat(PlayerHashSet.Instance.MoveSpeed,
                    (moveSpeed * stateMachine.InputReader.moveComposite.y), AnimationDampTime, Time.deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(PlayerHashSet.Instance.MoveSpeed, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);
        }

        if (stateMachine.Player.IsLockOn && moveSpeed < 0.7f)
        {
            float side = 0f;
            side = stateMachine.InputReader.moveComposite.x * 0.75f;
            stateMachine.Animator.SetFloat(PlayerHashSet.Instance.SideWalk, side, AnimationDampTime, Time.deltaTime);
        }
        else
        {
            stateMachine.Animator.SetFloat(PlayerHashSet.Instance.moveX, stateMachine.InputReader.moveComposite.x, AnimationDampTime, Time.deltaTime);
            stateMachine.Animator.SetFloat(PlayerHashSet.Instance.moveY, stateMachine.InputReader.moveComposite.y, AnimationDampTime, Time.deltaTime);
            stateMachine.Animator.SetFloat(PlayerHashSet.Instance.SideWalk, stateMachine.InputReader.moveComposite.x, AnimationDampTime, Time.deltaTime);
        }
        CalculateMoveDirection();
    }
    public override void FixedTick()
    {
        if (stateMachine.Player.IsLockOn)
        {
            if (moveSpeed > 0.5f)
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
        stateMachine.InputReader.onFlashSlashStart -= FlashSlash;
        stateMachine.InputReader.onRunStart -= RushAttack;


        stateMachine.InputReader.onLAttackStart -= Attack;
        stateMachine.InputReader.onLAttackCanceled -= ReleaseAttack;
        stateMachine.InputReader.onRAttackStart -= Gurad;
        stateMachine.InputReader.onJumpStart -= Dodge;

        stateMachine.InputReader.onRAttackCanceled -= ReleaseGuard;
        stateMachine.Animator.speed = 1f;
    }


    private void ReleaseAttack() { stateMachine.InputReader.clickCondition = false; }
    private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.isGuard, true); }
    public void ReleaseGuard() { stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false); }





    // �� ��ȭ�� �ε巴�� ����
    IEnumerator SmoothChangeSpeed()
    {
        float startSpeed = moveSpeed;
        float elapsedTime = 0.0f;

        while (elapsedTime < 0.1f)
        {
            moveSpeed = Mathf.Lerp(startSpeed, targetSpeed, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed = targetSpeed; // Ensure it reaches the target value at the end
    }
    private void Attack()
    {
        stateMachine.AutoTargetting.AutoTargeting();
        if (!stateMachine.Player.isBuff)
        {
            stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Attack);
            stateMachine.Animator.SetBool(PlayerHashSet.Instance.isMove, true);
            Player.Instance.isBuff = false;
        }
        else
        {
            Debug.Log("���� �������´� : " + stateMachine.Player.isBuff);
            stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
            stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.combAttack);

        }

    }
    private void Dodge()
    {
        if (stateMachine.InputReader.moveComposite.magnitude != 0f && !CoolTimeCounter.Instance.isDodgeUsed)
        {
            CoolTimeCounter.Instance.isDodgeUsed = true;
            if (stateMachine.InputReader.moveComposite.magnitude != 0)
            {
                stateMachine.Rigidbody.rotation = Quaternion.LookRotation(stateMachine.Velocity);
            }
            stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Dodge);
        }
    }

    // �ϼ�
    public void FlashSlash()
    {
        if (stateMachine.Animator.GetBool(PlayerHashSet.Instance.isFlashSlash) && Player.Instance.CP >= 20f)
        {
            stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.FlashSlash);
            Player.Instance.CP -= 25f;
        }
    }

    // �ð�����
    public void TimeSlash()
    {
        if (stateMachine.Animator.GetBool(PlayerHashSet.Instance.isTimeSlash))
        {
            if (!stateMachine.AutoTargetting.FindTarget()) return;
            stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.TimeSlashReady);
        }
    }

    private void RushAttack()
    {
        if (stateMachine.Animator.GetBool(PlayerHashSet.Instance.isRushAttack) && !CoolTimeCounter.Instance.isRushAttackUsed)
        {
            stateMachine.AutoTargetting.enabled = true;
            PlayerStateMachine.GetInstance().AutoTargetting.sphere.enabled = true;

            CoolTimeCounter.Instance.isRushAttackUsed = true;
            stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.RushAttack);
        }
    }
}




