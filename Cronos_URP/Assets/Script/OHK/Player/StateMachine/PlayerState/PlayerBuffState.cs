using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 플레이어의 버프상태를 정의하는 클래스
/// 
/// ohk    v1 
/// </summary>
public class PlayerBuffState : PlayerBaseState
{
	public PlayerBuffState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public float targetSpeed = 0.5f;

	private const float _animationDampTime = 0.1f;
	private float _moveSpeed = 0.5f;
	private float _releaseLockOn = 0f;
	private bool _isRelease = false;

	public override void Enter()
	{
		_stateMachine.Rigidbody.velocity = Vector3.zero;
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		_stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.GoIdle);

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
		// 이동키 입력을 확인한다.
		if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		{
			_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.IsMove);
		}

		// 플레이어의 cp 를 이동속도에 반영한다.
		_stateMachine.Animator.speed = _stateMachine.Player.CP * _stateMachine.Player.MoveCoefficient + 1f;
		_moveSpeed = 1f;

		_stateMachine.Player.SetSpeed(_moveSpeed);

		// 휠꾹
		if (_isRelease)
		{
			_releaseLockOn += Time.deltaTime;

			if (_releaseLockOn > 1f)
			{
				_stateMachine.autoTargetting.LockOff();
			}
		}

		// 애니메이터 moveSpeed의 파라메터의 값을 정한다.
		// 락온 상태일때 && 달리기가 아닐때
		if (_stateMachine.Player.IsLockOn && _moveSpeed < 0.6f)
		{
			// moveSpeed에 y값을곱해서 전방이동인지 후방이동인지 확인한다.
			_stateMachine.Animator.SetFloat(PlayerHashSet.Instance.IsMove,
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
	}

	public override void FixedTick()
	{
		Float();
	}

	public override void LateTick() { }

	public override void Exit()
	{
		_stateMachine.InputReader.onLAttackStart -= Attack;
		_stateMachine.InputReader.onRAttackStart -= Gurad;
		_stateMachine.InputReader.onJumpStart -= Dodge;
		_stateMachine.InputReader.onFlashSlashStart -= FlashSlash;
		_stateMachine.InputReader.onRunStart -= RushAttack;

		_stateMachine.InputReader.onLAttackCanceled -= ReleaseAttack;

		_stateMachine.InputReader.onRAttackCanceled -= ReleaseGuard;
		_stateMachine.Animator.speed = 1f;
	}

	private void Attack()
	{
		_stateMachine.autoTargetting.AutoTargeting();
		_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Attack);
	}

	private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsGuard, true); }

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

	private void FlashSlash()
	{
		if (_stateMachine.Animator.GetBool(PlayerHashSet.Instance.IsFlashSlash) && Player.Instance.CP >= 20f)
		{
			_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.FlashSlash);
			Player.Instance.CP -= 25f;
		}
	}

	private void ReleaseAttack() { _stateMachine.InputReader.clickCondition = false; }

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

	private void Dodge()
	{
		if (_stateMachine.InputReader.moveComposite.magnitude != 0f && _stateMachine.Animator.IsInTransition(_stateMachine.currentLayerIndex) && !CoolTimeCounter.Instance.IsDodgeUsed)
		{
			CoolTimeCounter.Instance.IsDodgeUsed = true;
			if (_stateMachine.InputReader.moveComposite.magnitude != 0)
			{
				_stateMachine.Rigidbody.rotation = Quaternion.LookRotation(_stateMachine.velocity);
			}
			_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Dodge);
		}
	}

	private void BuffOff()
	{
		_stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.IsMove);
	}
}
