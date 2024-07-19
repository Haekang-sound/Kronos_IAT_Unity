using System.Collections;
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
	private const float AnimationDampTime = 0.1f;

	float moveSpeed = 0.5f;
	public float targetSpeed = 0.5f;

	float releaseLockOn = 0f;

	public PlayerMoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

	public override void Enter()
	{
		stateMachine.InputReader.onSwitchingStart += Deceleration;
	}

	// state의 update라 볼 수 있지
	public override void Tick()
	{
		if (Input.GetKeyDown(KeyCode.V))
		{
			stateMachine.Player.CP += 1f;
		}

		// 플레이어의 cp 를 이동속도에 반영한다.
		stateMachine.Animator.speed = stateMachine.Player.CP * stateMachine.Player.MoveCoefficient + 1f;

		//moveSpeed = 0.5f;
		if (Input.GetButton("Run"))
		{
			moveSpeed = 1f;
		}
		else
		{
			stateMachine.StartCoroutine(SmoothChangeSpeed());
			//moveSpeed = 0.5f; 
		}

		stateMachine.Player.SetSpeed(moveSpeed);

		if (Input.GetMouseButtonDown(2))
		{
			// 락온 상태가 아니라면
			if (!stateMachine.Player.IsLockOn)
			{
				// 대상을 찾고
				stateMachine.Player.IsLockOn = stateMachine.AutoTargetting.FindTarget();
			}
			// 락온상태라면 락온을 해제한다.
			else
			{
				//stateMachine.AutoTargetting.LockOff();
				stateMachine.AutoTargetting.SwitchTarget();
			}
		}

		if (Input.GetMouseButton(2))
		{
			releaseLockOn += Time.deltaTime;

			if (releaseLockOn > 1f)
			{
				stateMachine.AutoTargetting.LockOff();
			}
		}
		else
		{
			releaseLockOn = 0f;
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
		stateMachine.InputReader.onSwitchingStart -= Deceleration;
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


}




