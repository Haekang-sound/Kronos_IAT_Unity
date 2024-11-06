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
			CalculateMoveDirection();   // ������ ����ϰ�
		}
		if (attackBool && stateMachine.currentStateInformable.normalizedTime > stateMachine.minf)
		{
			// NEXTCOMBO Ȱ��ȭ
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, true);
		}

		// ��Ŭ�� ������ �߿��� ��¡
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			float current = stateMachine.Animator.GetFloat(PlayerHashSet.Instance.Charge);
			stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, current + Time.deltaTime);
		}

		// ������������ ��¡���̴�
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			//��ǲ�߿� ����� ��������ҵ�
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.chargeAttack, true);
		}
		else
		{
			//��ǲ�߿� ����� ��������ҵ�
			stateMachine.Animator.SetBool(PlayerHashSet.Instance.chargeAttack, false);
		}

		// ��Ŭ������ ��¡ ��Ȱ��ȭ
		if (!stateMachine.InputReader.IsLAttackPressed)
		{
			stateMachine.Animator.SetFloat(PlayerHashSet.Instance.Charge, 0);
		}

		CalculateMoveDirection();   // ������ ����ϰ�

		Vector3 gravity = /*isOnSlope ? Vector3.zero :*/ Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);
		if (stateMachine.AutoTargetting.GetTarget() != null)
		{
			// Ÿ�ٰ� ĳ���ͻ����� �Ÿ��� 1���� ũ�ٸ� Ÿ�������� �ٰ�����.
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
			else // �����ϸ� �����.
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
		///Ʈ������ ���϶��� �ߵ�
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
		/// ��Ŭ����
		stateMachine.AutoTargetting.AutoTargeting();
		attackBool = true;

		if (attackBool && currentStateInfo.normalizedTime > minFrame)
		{
			// NEXTCOMBO Ȱ��ȭ
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
			// ���� ���
			CoolTimeCounter.Instance.isDodgeUsed = true;        // ��Ÿ�� ���üũ�Ѵ�.
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
