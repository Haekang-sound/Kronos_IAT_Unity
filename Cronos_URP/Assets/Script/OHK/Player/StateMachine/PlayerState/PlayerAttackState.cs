using UnityEngine;
public class PlayerAttackState : PlayerBaseState
{
	//private bool ismove = false;
	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int chargeHash = Animator.StringToHash("Charge");
	private readonly int chargeAttackHash = Animator.StringToHash("chargeAttack");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private readonly int guradHash = Animator.StringToHash("isGuard");
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

		stateMachine.Animator.SetBool(nextComboHash, false);
		stateMachine.Animator.SetBool(guradHash, false);
		stateMachine.Animator.ResetTrigger("Attack");
		stateMachine.Animator.ResetTrigger("Rattack");
		stateMachine.Animator.ResetTrigger("ParryAttack");

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
			stateMachine.Animator.SetBool(nextComboHash, true);
		}

		// 좌클릭 누르는 중에는 차징
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			float current = stateMachine.Animator.GetFloat(chargeHash);
			stateMachine.Animator.SetFloat(chargeHash, current + Time.deltaTime);
		}

		// 누르고있으면 차징중이다
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			//인풋중에 뭐라고 정해줘야할듯
			stateMachine.Animator.SetBool(chargeAttackHash, true);
		}
		else
		{
			//인풋중에 뭐라고 정해줘야할듯
			stateMachine.Animator.SetBool(chargeAttackHash, false);
		}

		// 좌클릭땔때 차징 비활성화
		if (!stateMachine.InputReader.IsLAttackPressed)
		{
			stateMachine.Animator.SetFloat(chargeHash, 0);
		}

		CalculateMoveDirection();   // 방향을 계산하고

		Vector3 gravity = /*isOnSlope ? Vector3.zero :*/ Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);
		if (stateMachine.MoveForce > 1f && stateMachine.Animator.deltaPosition != null)
		{
			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) * stateMachine.MoveForce + gravity;
		}
		else if (stateMachine.Animator.deltaPosition != null)
		{
			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) + gravity;
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
		stateMachine.Animator.SetFloat(chargeHash, 0);
		stateMachine.Animator.SetBool(chargeAttackHash, false);
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
			stateMachine.Animator.SetBool(nextComboHash, true);
		}
	}
	private void Dodge()
	{
		if (!CoolTimeCounter.Instance.isDodgeUsed)
		{
			// 사용될 경우
			CoolTimeCounter.Instance.isDodgeUsed = true;        // 쿨타임 사용체크한다.
			stateMachine.Animator.SetBool(nextComboHash, false);    // 
																	//stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
			stateMachine.Animator.SetTrigger(dodgeHash);
		}
	}
	private void Gurad()
	{
		if (stateMachine.IsRattack)
		{
			stateMachine.IsRattack = false;
			return;
		}
		stateMachine.Animator.SetBool(guradHash, true);
	}

}
