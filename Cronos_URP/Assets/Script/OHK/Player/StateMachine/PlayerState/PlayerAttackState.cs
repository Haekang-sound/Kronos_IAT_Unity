using UnityEngine;
public class PlayerAttackState : PlayerBaseState
{
	//private bool ismove = false;
	public PlayerAttackState(PlayerStateMachine stateMachine) : base(stateMachine) { }
	private readonly int moveHash = Animator.StringToHash("isMove");
	private readonly int nextComboHash = Animator.StringToHash("NextCombo");
	private readonly int chargeHash = Animator.StringToHash("Charge");
	private readonly int chargeAttackHash = Animator.StringToHash("chargeAttack");
	private readonly int dodgeHash = Animator.StringToHash("Dodge");
	private readonly int guradHash = Animator.StringToHash("isGuard");

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
// 
		stateMachine.Animator.SetBool(nextComboHash, false);
		stateMachine.Animator.ResetTrigger("Attack");

		stateMachine.InputReader.onLAttackStart += Attack;
		stateMachine.InputReader.onRAttackStart += Gurad;
		stateMachine.InputReader.onJumpStart += Dodge;
	}
	public override void Tick()
	{

		/// 좌클릭시
// 		if ((Input.GetKeyDown(KeyCode.Mouse0) && stateMachine.currentStateInformable.normalizedTime < stateMachine.minf))
// 		{
// 			attackBool = true;
// 		}
		//if ((Input.GetKeyDown(KeyCode.Mouse0) || attackBool) && stateMachine.currentStateInformable.normalizedTime > stateMachine.minf)
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
	}
	public override void FixedTick()
	{
		// Implement code that processes and affects root motion
		// 애니메이터에서 루트모션을 받아온다. 
		Vector3 rootMotion = stateMachine.Animator.deltaPosition;
		rootMotion.y = 0;
		if (IsOnSlope())
		{
			stateMachine.Rigidbody.velocity = AdjustDirectionToSlope(rootMotion) * stateMachine.MoveForce;
			//Debug.Log(stateMachine.Rigidbody.velocity);
		}
		else
		{
			stateMachine.Rigidbody.velocity = rootMotion * stateMachine.MoveForce;

		}
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
        /// 좌클릭시
        //if (currentStateInfo.normalizedTime < minFrame)
        //{
        stateMachine.AutoTargetting.AutoTargeting();
        attackBool = true;
		//}
		if (attackBool && currentStateInfo.normalizedTime > minFrame)
		{
			// NEXTCOMBO 활성화
			stateMachine.Animator.SetBool(nextComboHash, true);
		}
	}
	private void Dodge()
	{
		if (stateMachine.Velocity.magnitude != 0f)
		{
			stateMachine.Animator.SetBool(nextComboHash, false);
			stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
			stateMachine.Animator.SetTrigger(dodgeHash);
		}
	}
	private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(guradHash, true); }

}
