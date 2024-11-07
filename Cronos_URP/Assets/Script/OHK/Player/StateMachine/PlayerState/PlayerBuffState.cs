using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerBuffState : PlayerBaseState
{

	public PlayerBuffState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	private const float AnimationDampTime = 0.1f;

	float moveSpeed = 0.5f;
	public float targetSpeed = 0.5f;
	float releaseLockOn = 0f;
	bool isRelease = false;

	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
        stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
        stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.goIdle);

		stateMachine.InputReader.onDecelerationStart += Deceleration;
		stateMachine.InputReader.onFlashSlashStart += FlashSlash;
		stateMachine.InputReader.onRunStart += RushAttack;

		stateMachine.InputReader.onLAttackStart += Attack;
        stateMachine.InputReader.onRAttackStart += Gurad;
        stateMachine.InputReader.onJumpStart += Dodge;

		stateMachine.InputReader.onLockOnStart += LockOn;
		stateMachine.InputReader.onLockOnPerformed += ReleaseLockOn;
		stateMachine.InputReader.onLockOnCanceled += ReleaseReset;

		stateMachine.InputReader.onLAttackCanceled += ReleaseAttack;

		stateMachine.InputReader.onRAttackCanceled += ReleaseGuard;

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

	private void FlashSlash()
	{
		if (stateMachine.Animator.GetBool(PlayerHashSet.Instance.isFlashSlash) && Player.Instance.CP >= 20f)
		{
			stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.FlashSlash);
			Player.Instance.CP -= 25f;
		}
	}

	public override void Tick()
	{

		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.isMove);
		    //stateMachine.Animator.SetBool(PlayerHashSet.Instance.isEnforced, true);
		}

		// �÷��̾��� cp �� �̵��ӵ��� �ݿ��Ѵ�.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;
		moveSpeed = 1f;

		stateMachine.Player.SetSpeed(moveSpeed);

		// �ٲ�
		if (isRelease)
		{
			releaseLockOn += Time.deltaTime;

			if (releaseLockOn > 1f)
			{
				stateMachine.AutoTargetting.LockOff();
			}
		}

		// �ִϸ����� movespeed�� �Ķ������ ���� ���Ѵ�.
		// ���� �����϶� && �޸��Ⱑ �ƴҶ�
		if (stateMachine.Player.IsLockOn && moveSpeed < 0.6f)
		{
			// moveSpeed�� y�������ؼ� �����̵����� �Ĺ��̵����� Ȯ���Ѵ�.
			stateMachine.Animator.SetFloat(PlayerHashSet.Instance.isMove,
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
		//CalculateMoveDirection();   // ������ ����ϰ�

	}
	public override void FixedTick()
	{
		Float();
// 		if (stateMachine.Player.IsLockOn)
// 		{
// 			if (moveSpeed > 0.5f)
// 			{
// 				FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
// 			}
// 		}
// 		else
// 		{
// 			FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
// 		}
// 		Move();                     // �̵��Ѵ�.	
	}
    public override void LateTick()
	{
    }
	public override void Exit()
	{
        stateMachine.InputReader.onLAttackStart -= Attack;
        stateMachine.InputReader.onRAttackStart -= Gurad;
        stateMachine.InputReader.onJumpStart -= Dodge;
		//stateMachine.Animator.SetBool(BuffHash, false);
		stateMachine.InputReader.onDecelerationStart -= Deceleration;
		stateMachine.InputReader.onLockOnStart -= LockOn;
		stateMachine.InputReader.onLockOnPerformed -= ReleaseLockOn;
		stateMachine.InputReader.onLockOnCanceled -= ReleaseReset;

		stateMachine.InputReader.onLAttackCanceled -= ReleaseAttack;

		stateMachine.InputReader.onRAttackCanceled -= ReleaseGuard;
		stateMachine.Animator.speed = 1f;
	}
    private void Attack()
    {
        stateMachine.AutoTargetting.AutoTargeting();
        stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Attack);
    }

	private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.isGuard, true); }



	private void ReleaseAttack() { stateMachine.InputReader.clickCondition = false; }
	public void ReleaseGuard() { stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false); }
	private void LockOn()
	{
		Debug.Log("����");
		// ���� ���°� �ƴ϶��
		if (!stateMachine.Player.IsLockOn)
		{
			// ����� ã��
			bool temp = stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			Debug.Log(temp);
		}
		// ���»��¶�� Ÿ���� �����Ѵ�.
		else
		{
			stateMachine.AutoTargetting.SwitchTarget();
		}
	}

	private void ReleaseLockOn()
	{
		isRelease = true;

		//Debug.Log("��������");
		releaseLockOn += Time.deltaTime;

		if (releaseLockOn > 1f)
		{
			stateMachine.AutoTargetting.LockOff();
		}
	}
	private void ReleaseReset()
	{
		isRelease = false;
		releaseLockOn = 0f;
	}

	private void Deceleration()
	{
		if (stateMachine.Player.CP >= 100 && stateMachine.Animator.GetBool(PlayerHashSet.Instance.isTimeStop))
		{
			stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.TimeStop);
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}
		else if (stateMachine.Player.IsDecreaseCP && stateMachine.Animator.GetBool(PlayerHashSet.Instance.isCPBoomb))
		{
			stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.CPBoomb);
		}

	}
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

	private void Dodge()
	{
		if (stateMachine.InputReader.moveComposite.magnitude != 0f && stateMachine.Animator.IsInTransition(stateMachine.currentLayerIndex) && !CoolTimeCounter.Instance.isDodgeUsed)
		{
			CoolTimeCounter.Instance.isDodgeUsed = true;
			stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Dodge);
		}
	}

	private void BuffOff()
	{
		stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.isMove);
	}
}
