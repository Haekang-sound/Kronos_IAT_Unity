using System.Collections;
using System.Collections.Generic;
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

	private const float AnimationDampTime = 0.1f;

	float moveSpeed = 0.5f;
	public float targetSpeed = 0.5f;
	float releaseLockOn = 0f;
	bool isRelease = false;
	bool isRun = false;

	Vector3 totalMove;
	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		stateMachine.InputReader.onDecelerationStart += Deceleration;
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

	// state의 update라 볼 수 있지
	public override void Tick()
	{
// 		Vector3 rootMotion = stateMachine.Animator.deltaPosition;
// 		rootMotion.y = 0;
		//totalMove += rootMotion;

		// 플레이어의 cp 를 이동속도에 반영한다.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		//stateMachine.Animator.SetFloat("animSpeed", stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f);

		if (stateMachine.Player.IsLockOn)
		{
			if (isRun)
			{
				moveSpeed = 1f;
			}
			else
			{
				stateMachine.StartCoroutine(SmoothChangeSpeed());
			}
		}
		else
		{
			moveSpeed = 1f;
		}


		stateMachine.Player.SetSpeed(moveSpeed);

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
											/*Mathf.Abs(stateMachine.InputReader.moveComposite.y) > 0f ? moveSpeed :*/
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
// 		//Move(totalMove);                     // 이동한다.	
// 		Move(rootMotion);                     // 이동한다.	
//         totalMove = Vector3.zero;


    }
    public override void FixedTick()
	{
		Float();
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
		//		Float();/// floatingcapsule실험중
		//Move(totalMove);                     // 이동한다.	
		//stateMachine.Rigidbody.velocity = totalMove;

	}

	public override void LateTick() { }

	public override void Exit()
	{
		stateMachine.InputReader.onDecelerationStart -= Deceleration;
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
		Debug.Log("누름");
		// 락온 상태가 아니라면
		if (!stateMachine.Player.IsLockOn)
		{
			// 대상을 찾고
			stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
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
		if (stateMachine.Player.CP >= 100)
		{
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
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
		PlayerStateMachine.GetInstance().Animator.SetBool(attackHash, true); 
	}
	private void Dodge()
	{
		if (stateMachine.InputReader.moveComposite.magnitude != 0f)
		{
			stateMachine.Animator.SetTrigger(dodgeHash);
		}
	}
}




