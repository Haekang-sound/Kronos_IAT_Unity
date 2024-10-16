using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

// 플레이어 기본상태를 상속받은 movestate
public class PlayerMoveState : PlayerBaseState
{
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int SideWalkHash = Animator.StringToHash("SideWalk");
	private readonly int moveXHash = Animator.StringToHash("moveX");
	private readonly int moveYHash = Animator.StringToHash("moveY");
	private readonly int attackHash = Animator.StringToHash("Attack");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private readonly int guradHash = Animator.StringToHash("isGuard");
	private readonly int timeStopHash = Animator.StringToHash("TimeStop");
	private readonly int CPBoombHash = Animator.StringToHash("CPBoomb");

	private const float AnimationDampTime = 0.1f;

	float moveSpeed = 0.5f;
	public float targetSpeed = 0.5f;
	float releaseLockOn = 0f;
	bool isRelease = false;
	bool isRun = false;
	float timeLine;
	bool timeslash = false;

	Vector3 totalMove;
	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		stateMachine.InputReader.onDecelerationStart += Deceleration;
		stateMachine.InputReader.onFlashSlashStart += FlashSlash;

		stateMachine.InputReader.onLockOnStart += LockOn;
		stateMachine.InputReader.onLockOnPerformed += ReleaseLockOn;
		stateMachine.InputReader.onLockOnCanceled += ReleaseReset;
		stateMachine.InputReader.onRunStart += Run;
		stateMachine.InputReader.onRunCanceled += StopRun;

		stateMachine.InputReader.onLAttackStart += Attack;
		stateMachine.InputReader.onLAttackCanceled += ReleaseAttack;
		stateMachine.InputReader.onRAttackStart += Gurad;
		stateMachine.InputReader.onJumpStart += Dodge;

		stateMachine.InputReader.onRAttackCanceled += ReleaseGuard;



	}

	// 시간베기
	public void FlashSlash()
	{
		Debug.Log("심판의 일섬!");
		stateMachine.Animator.SetTrigger("FlashSlash");
	}

	// state의 update라 볼 수 있지
	public override void Tick()
	{
		// 플레이어의 cp 를 이동속도에 반영한다.
		//stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;
		moveSpeed = 1f;

		stateMachine.Player.SetSpeed(moveSpeed);

		if(stateMachine.Velocity.magnitude > 0f)
		{
			stateMachine.Animator.SetBool("isMove",true);
		}
		else
		{
			stateMachine.Animator.SetBool("isMove",false);

		}

		// 시간베기 테스트용
		if (Input.GetKeyDown(KeyCode.R))
		{
			timeslash = true;
		}
		// 시간베기 테스트용
		if (timeslash)
		{
			timeLine += Time.deltaTime;
			stateMachine.Rigidbody.AddForce(stateMachine.transform.forward * stateMachine.Player.TimeSlashCurve.Evaluate(timeLine / 0.5f), ForceMode.Impulse);
			if (timeLine > 0.5f)
			{
				timeslash = false;
				timeLine = 0f;
			}
		}

		// 휠꾹
		if (isRelease)
		{
			releaseLockOn += Time.deltaTime;

			if (releaseLockOn > 1f)
			{
				stateMachine.AutoTargetting.LockOff();
			}
		}

		// 애니메이터 movespeed의 파라메터의 값을 정한다.
		// 락온 상태일때 && 달리기가 아닐때
		if (stateMachine.Player.IsLockOn && moveSpeed < 0.6f)
		{
			// moveSpeed에 y값을곱해서 전방이동인지 후방이동인지 확인한다.
			stateMachine.Animator.SetFloat(MoveSpeedHash,
											(moveSpeed * stateMachine.InputReader.moveComposite.y), AnimationDampTime, Time.deltaTime);
		}
		else
		{
			stateMachine.Animator.SetFloat(MoveSpeedHash, stateMachine.InputReader.moveComposite.sqrMagnitude > 0f ? moveSpeed : 0f, AnimationDampTime, Time.deltaTime);
		}

		if (stateMachine.Player.IsLockOn && moveSpeed < 0.7f)
		{
			float side = 0f;
			side = stateMachine.InputReader.moveComposite.x * 0.75f;
			stateMachine.Animator.SetFloat(SideWalkHash, side, AnimationDampTime, Time.deltaTime);
		}
		else
		{
			stateMachine.Animator.SetFloat(moveXHash, stateMachine.InputReader.moveComposite.x, AnimationDampTime, Time.deltaTime);
			stateMachine.Animator.SetFloat(moveYHash, stateMachine.InputReader.moveComposite.y, AnimationDampTime, Time.deltaTime);
			stateMachine.Animator.SetFloat(SideWalkHash, stateMachine.InputReader.moveComposite.x, AnimationDampTime, Time.deltaTime);
		}
		CalculateMoveDirection();   // 방향을 계산하고
	}
	public override void FixedTick()
	{
		if (stateMachine.Player.IsLockOn)
		{
			if (moveSpeed > 0.5f)
			{
				FaceMoveDirection();        // 캐릭터 방향을 바꾸고
			}
		}
		else
		{
			FaceMoveDirection();        // 캐릭터 방향을 바꾸고
		}
		Move();                     // 이동한다.	

	}

	public override void LateTick() { }

	public override void Exit()
	{
		stateMachine.InputReader.onDecelerationStart -= Deceleration;
		stateMachine.InputReader.onFlashSlashStart -= FlashSlash;

		stateMachine.InputReader.onLockOnStart -= LockOn;
		stateMachine.InputReader.onLockOnPerformed -= ReleaseLockOn;
		stateMachine.InputReader.onLockOnCanceled -= ReleaseReset;
		stateMachine.InputReader.onRunStart -= Run;
		stateMachine.InputReader.onRunCanceled -= StopRun;

		stateMachine.InputReader.onLAttackStart -= Attack;
		stateMachine.InputReader.onLAttackCanceled -= ReleaseAttack;
		stateMachine.InputReader.onRAttackStart -= Gurad;
		stateMachine.InputReader.onJumpStart -= Dodge;

		stateMachine.InputReader.onRAttackCanceled -= ReleaseGuard;
		stateMachine.Animator.speed = 1f;
	}


	private void ReleaseAttack() { stateMachine.InputReader.clickCondition = false; }
	private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(guradHash, true); }
	public void ReleaseGuard() { stateMachine.Animator.SetBool(guradHash, false); }
	private void Run() { isRun = true; }
	private void StopRun() { isRun = false; }
	private void LockOn()
	{
		// 락온 상태가 아니라면
		if (!stateMachine.Player.IsLockOn)
		{
			// 대상을 찾고
			bool temp = stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			Debug.Log(temp);
		}
		// 락온상태라면 타겟을 변경한다.
		else
		{
			stateMachine.AutoTargetting.SwitchTarget();
		}
	}

	private void ReleaseLockOn()
	{
		isRelease = true;

		//Debug.Log("누르는중");
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
		if (stateMachine.Player.CP >= 100 && stateMachine.Animator.GetBool("isTimeStop"))
		{
			stateMachine.Animator.SetTrigger(timeStopHash);
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}
		else if(stateMachine.Player.IsDecreaseCP && stateMachine.Animator.GetBool("isCPBoomb"))
		{
			stateMachine.Animator.SetTrigger(CPBoombHash);
		}

	}
	// 값 변화를 부드럽게 주자
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
		stateMachine.Animator.SetBool(attackHash, true);
	}
	private void Dodge()
	{
		if (stateMachine.Player.CP < 10f)
		{
			return;
		}
		stateMachine.Player.CP -= 10f;
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			stateMachine.Animator.SetTrigger(dodgeHash);
		}
	}
}




