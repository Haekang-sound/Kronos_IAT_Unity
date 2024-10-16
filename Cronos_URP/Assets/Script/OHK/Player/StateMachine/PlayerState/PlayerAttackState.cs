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
		stateMachine.Animator.ResetTrigger("Attack");
		stateMachine.Animator.ResetTrigger("Rattack");

		stateMachine.InputReader.onLAttackStart += Attack;
		stateMachine.InputReader.onRAttackStart += Gurad;
		stateMachine.InputReader.onJumpStart += Dodge;
	}
	public override void Tick()
	{
		if (attackBool && stateMachine.currentStateInformable.normalizedTime > stateMachine.minf)
		{
			// NEXTCOMBO Ȱ��ȭ
			stateMachine.Animator.SetBool(nextComboHash, true);
		}

		// ��Ŭ�� ������ �߿��� ��¡
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			float current = stateMachine.Animator.GetFloat(chargeHash);
			stateMachine.Animator.SetFloat(chargeHash, current + Time.deltaTime);
		}

		// ������������ ��¡���̴�
		if (stateMachine.InputReader.IsLAttackPressed)
		{
			//��ǲ�߿� ����� ��������ҵ�
			stateMachine.Animator.SetBool(chargeAttackHash, true);
		}
		else
		{
			//��ǲ�߿� ����� ��������ҵ�
			stateMachine.Animator.SetBool(chargeAttackHash, false);
		}

		// ��Ŭ������ ��¡ ��Ȱ��ȭ
		if (!stateMachine.InputReader.IsLAttackPressed)
		{
			stateMachine.Animator.SetFloat(chargeHash, 0);
		}

		CalculateMoveDirection();   // ������ ����ϰ�


		if (stateMachine.MoveForce > 1f && stateMachine.Animator.deltaPosition != null)
		{
			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime) * stateMachine.MoveForce;
		}
		else if (stateMachine.Animator.deltaPosition != null)
		{
			stateMachine.Rigidbody.velocity = (stateMachine.Animator.deltaPosition / Time.deltaTime);
		}
	}
	public override void FixedTick()
	{
		///Ʈ������ ���϶��� �ߵ�
		if (stateMachine.Animator.IsInTransition(stateMachine.currentLayerIndex))
		{
			FaceMoveDirection();
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
	}

	private void Attack()
	{
		/// ��Ŭ����
		//if (currentStateInfo.normalizedTime < minFrame)
		//{
		stateMachine.AutoTargetting.AutoTargeting();
		attackBool = true;
		//}
		if (attackBool && currentStateInfo.normalizedTime > minFrame)
		{
			// NEXTCOMBO Ȱ��ȭ
			stateMachine.Animator.SetBool(nextComboHash, true);
		}
	}
	private void Dodge()
	{
		if (stateMachine.Player.CP < 10f)
		{
			return;
		}
		stateMachine.Player.CP -= 10f;
		if (stateMachine.Velocity.magnitude != 0f)
		{
			stateMachine.Animator.SetBool(nextComboHash, false);
			stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
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
