using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering.Universal;

public class PlayerBuffState : PlayerBaseState
{

	public PlayerBuffState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    private readonly int attackHash = Animator.StringToHash("Attack");
    private readonly int BuffHash = Animator.StringToHash("Buff");
    private readonly int moveHash = Animator.StringToHash("isMove");
    private readonly int idleHash = Animator.StringToHash("goIdle");
    private readonly int dodgeHash = Animator.StringToHash("Dodge");
    private readonly int guradHash = Animator.StringToHash("isGuard");
	private readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
	private readonly int SideWalkHash = Animator.StringToHash("SideWalk");
	private readonly int moveXHash = Animator.StringToHash("moveX");
	private readonly int moveYHash = Animator.StringToHash("moveY");
	private readonly int timeStopHash = Animator.StringToHash("TimeStop");
	private readonly int CPBoombHash = Animator.StringToHash("CPBoomb");
	[SerializeField] private float buffTimer = 3f;
	public float buffTime = 3f;


	private const float AnimationDampTime = 0.1f;

	float moveSpeed = 0.5f;
	public float targetSpeed = 0.5f;
	float releaseLockOn = 0f;
	bool isRelease = false;
	bool isRun = false;
	float timeLine;
	bool timeslash = false;

	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
        stateMachine.Animator.ResetTrigger(attackHash);
        stateMachine.Animator.ResetTrigger(idleHash);
        //stateMachine.Animator.SetBool(BuffHash, true);
        buffTimer = 0f;

        stateMachine.InputReader.onLAttackStart += Attack;
        stateMachine.InputReader.onRAttackStart += Gurad;
        stateMachine.InputReader.onJumpStart += Dodge;

		stateMachine.InputReader.onDecelerationStart += Deceleration;
		stateMachine.InputReader.onLockOnStart += LockOn;
		stateMachine.InputReader.onLockOnPerformed += ReleaseLockOn;
		stateMachine.InputReader.onLockOnCanceled += ReleaseReset;
		stateMachine.InputReader.onRunStart += Run;
		stateMachine.InputReader.onRunCanceled += StopRun;

		stateMachine.InputReader.onLAttackCanceled += ReleaseAttack;

		stateMachine.InputReader.onRAttackCanceled += ReleaseGuard;

	}
	public override void Tick()
	{

        buffTimer += Time.deltaTime;


        // Ư�� ������ ������ �� �ִϸ��̼��� �����ϰ� targetStateName���� ��ȯ
         if (buffTimer > buffTime)
         {
             stateMachine.Animator.SetTrigger(moveHash);
         }

		//         if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
		//         {
		//             stateMachine.Animator.SetBool(moveHash, true);
		//         }

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
		CalculateMoveDirection();   // ������ ����ϰ�

	}
	public override void FixedTick()
	{
		if (stateMachine.Player.IsLockOn)
		{
			if (moveSpeed > 0.5f)
			{
				FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
			}
		}
		else
		{
			FaceMoveDirection();        // ĳ���� ������ �ٲٰ�
		}
		Move();                     // �̵��Ѵ�.	
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
		stateMachine.InputReader.onRunStart -= Run;
		stateMachine.InputReader.onRunCanceled -= StopRun;

		stateMachine.InputReader.onLAttackCanceled -= ReleaseAttack;

		stateMachine.InputReader.onRAttackCanceled -= ReleaseGuard;
		stateMachine.Animator.speed = 1f;

	}
    private void Attack()
    {
        stateMachine.AutoTargetting.AutoTargeting();
        stateMachine.Animator.SetTrigger(attackHash);
    }

	private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(guradHash, true); }



	private void ReleaseAttack() { stateMachine.InputReader.clickCondition = false; }
	public void ReleaseGuard() { stateMachine.Animator.SetBool(guradHash, false); }
	private void Run() { isRun = true; }
	private void StopRun() { isRun = false; }
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
		if (stateMachine.Player.CP >= 100 && stateMachine.Animator.GetBool("isTimeStop"))
		{
			stateMachine.Animator.SetTrigger(timeStopHash);
			BulletTime.Instance.DecelerateSpeed();
			stateMachine.Player.IsDecreaseCP = true;
		}
		else if (stateMachine.Player.IsDecreaseCP && stateMachine.Animator.GetBool("isCPBoomb"))
		{
			stateMachine.Animator.SetTrigger(CPBoombHash);
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
