using TMPro;
using UnityEditorInternal;
using UnityEngine;
public class PlayerRushAttackState : PlayerBaseState
{
	//private bool ismove = false;
	public PlayerRushAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private readonly int guradHash = Animator.StringToHash("isGuard");
	Vector3 totalMove;
	[SerializeField] float moveForce;


	public float hitStopTime;
	[Range(0.0f, 1.0f)] public float minFrame;
	AnimatorStateInfo currentStateInfo;
	Vector3 TargetPosition;
	public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
		stateMachine.MoveForce = moveForce;
		stateMachine.HitStop.hitStopTime = hitStopTime;

		stateMachine.Animator.SetBool(guradHash, false);
		stateMachine.Animator.ResetTrigger("Attack");
		stateMachine.Animator.ResetTrigger("Rattack");
		stateMachine.Animator.ResetTrigger("ParryAttack");

		stateMachine.InputReader.onRAttackStart += Gurad;
		stateMachine.InputReader.onJumpStart += Dodge;

		stateMachine.AutoTargetting.AutoTargeting();

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

		Vector3 gravity = /*isOnSlope ? Vector3.zero :*/ Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);

		if (stateMachine.AutoTargetting.GetTarget() != null)
		{
			Debug.Log((TargetPosition - stateMachine.transform.position).magnitude);
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
			if (stateMachine.AutoTargetting.GetTarget() != null)
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
		stateMachine.InputReader.onRAttackStart -= Gurad;
		stateMachine.InputReader.onJumpStart -= Dodge;

		stateMachine.GroundChecker.ToggleChecker = true;
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
