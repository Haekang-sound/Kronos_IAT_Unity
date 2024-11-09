using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerDodgeAttackState : PlayerBaseState
{
	//private bool ismove = false;
	public PlayerDodgeAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	Vector3 totalMove;
	[SerializeField] float moveForce;

	Vector3 TargetPosition;
	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;
	public override void Enter()
	{
		stateMachine.AutoTargetting.AutoTargeting();
		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;
		
		stateMachine.Animator.SetBool(PlayerHashSet.Instance.NextCombo, false);
		stateMachine.Animator.SetBool(PlayerHashSet.Instance.isGuard, false);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Attack);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.Rattack);
		stateMachine.Animator.ResetTrigger(PlayerHashSet.Instance.ParryAttack);

		stateMachine.GroundChecker.ToggleChecker = false;
	}
	public override void Tick()
	{
		if (stateMachine.AutoTargetting.GetTarget() != null)
		{
			TargetPosition = stateMachine.AutoTargetting.GetTarget().GetComponent<LockOn>().TargetTransform.position;
		}
		else
		{
			CalculateMoveDirection();   // ������ ����ϰ�
		}
		

		CalculateMoveDirection();   // ������ ����ϰ�

		Vector3 gravity = Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);
		if (Time.deltaTime == 0f) return;
		else if (stateMachine.AutoTargetting.GetTarget() != null)
		{
			// Ÿ�ٰ� ĳ���ͻ����� �Ÿ��� 1���� ũ�ٸ� Ÿ�������� �ٰ�����.
			if ((TargetPosition - stateMachine.transform.position).magnitude > 1f)
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
		//if (stateMachine.Animator.IsInTransition(stateMachine.currentLayerIndex))
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
		stateMachine.GroundChecker.ToggleChecker = true;
	}

}
