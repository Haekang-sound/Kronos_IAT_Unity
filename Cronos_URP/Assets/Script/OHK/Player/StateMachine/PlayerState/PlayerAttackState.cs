using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerAttackState : PlayerBaseState
{
	//private bool ismove = false;
	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	Vector3 totalMove;
	[SerializeField] float moveForce;
	bool attackBool = false;

	Vector3 TargetPosition;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;
	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
		attackBool = false;
		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, false);
		stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.ParryAttack);

		stateMachine.InputReader.onLAttackStart += Attack;
		stateMachine.InputReader.onRAttackStart += Gurad;
		stateMachine.InputReader.onJumpStart += Dodge;

		stateMachine.GroundChecker.ToggleChecker = false;
	}
	public override void Tick()
	{
		if (stateMachine.AutoTargetting.GetTarget() != null)
		{
			TargetPosition = stateMachine.AutoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position - stateMachine.transform.forward * 1f;
		}
		else
		{
			CalculateMoveDirection();   // 방향을 계산하고
		}
		if (attackBool && stateMachine.currentStateInformable.normalizedTime > stateMachine.minf)
		{
			// NEXTCOMBO 활성화
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, true);
		}

		// 좌클릭 누르는 중에는 차징
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			float current = stateMachine.Animator.GetFloat(PlayerHashSet.Instance.Charge);
			stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, current + Time.deltaTime);
		}

		// 누르고있으면 차징중이다
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			//인풋중에 뭐라고 정해줘야할듯
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.chargeAttack, true);
		}
		else
		{
			//인풋중에 뭐라고 정해줘야할듯
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.chargeAttack, false);
		}

		// 좌클릭땔때 차징 비활성화
		if (!stateMachine.InputReader.IsLAttackPressed)
		{
			stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, 0);
		}

		CalculateMoveDirection();   // 방향을 계산하고

		Vector3 gravity = /*isOnSlope ? Vector3.zero :*/ Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);
		if (stateMachine.AutoTargetting.GetTarget() != null)
		{
			// 타겟과 캐릭터사이의 거리가 1보다 크다면 타겟쪽으로 다가간다.
			if ((TargetPosition - stateMachine.transform.position).magnitude 
				> stateMachine.Player.targetdistance)
			{
				if (stateMachine.MoveForce > 1f && stateMachine.Animator.deltaPosition != null)
				{
					stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) * stateMachine.MoveForce + gravity;
				}
				else if (stateMachine.Animator.deltaPosition != null)
				{
					stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) + gravity;
				}
			}
			else // 도착하면 멈춘다.
			{
				stateMachine.Rigidbody.velocity = Vector3.zero;
			}

		}
		else
		{
			if (stateMachine.MoveForce > 1f && stateMachine.Animator.deltaPosition != null)
			{
				stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) * stateMachine.MoveForce + gravity;
			}
			else if (stateMachine.Animator.deltaPosition != null)
			{
				stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) + gravity;
			}
		}
	}
	public override void FixedTick()
	{
		///트랜지션 중일때만 발동
		if (stateMachine.Animator.IsInTransition(stateMachine.currentLayerIndex))
		{
			if (stateMachine.AutoTargetting.GetTarget() != null && stateMachine.InputReader.moveComposite.magnitude == 0f)
			{
				FaceMoveDirection((TargetPosition - stateMachine.transform.position).normalized);
			}
			else
			{
				FaceMoveDirection();
			}
		}
		Float();
	}
	public override void LateTick() { }
	public override void Exit()
	{
		stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, 0);
		stateMachine.Animator.SetBool(PlayerHashSet.Instance.chargeAttack, false);
		stateMachine.InputReader.onLAttackStart -= Attack;
		stateMachine.InputReader.onRAttackStart -= Gurad;
		stateMachine.InputReader.onJumpStart -= Dodge;

		stateMachine.GroundChecker.ToggleChecker = true;
	}

	private void Attack()
	{
		/// 좌클릭시
		stateMachine.AutoTargetting.AutoTargeting();
		attackBool = true;

		if (attackBool && currentStateInfo.normalizedTime > minFrame)
		{
			// NEXTCOMBO 활성화
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, true);
		}
		else
		{
			Player.Instance.isBuff = false;
			Player.Instance.buffTimer = 0f;
		}
	}
	private void Dodge()
	{
		if (!CoolTimeCounter.Instance.isDodgeUsed && !stateMachine.DodgeBool)
		{
			// 사용될 경우
			CoolTimeCounter.Instance.isDodgeUsed = true;        // 쿨타임 사용체크한다.
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, false);    // 
																	//stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
			stateMachine.Animator.SetTrigger(PlayerHashSet.Instance.Dodge);
		}
	}
	private void Gurad()
	{
		if (stateMachine.IsRattack)
		{
			stateMachine.IsRattack = false;
			return;
		}
		stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, true);
	}

}
