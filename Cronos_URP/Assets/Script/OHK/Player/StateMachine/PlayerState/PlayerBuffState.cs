using UnityEditorInternal;
using UnityEngine;

public class PlayerBuffState : PlayerBaseState
{

	public PlayerBuffState(PlayerStateMachine stateMachine) : base(stateMachine) { }
    private readonly int attackHash = Animator.StringToHash("Attack");
    private readonly int BuffHash = Animator.StringToHash("Buff");
    private readonly int moveHash = Animator.StringToHash("isMove");
    private readonly int idleHash = Animator.StringToHash("goIdle");
    private readonly int dodgeHash = Animator.StringToHash("Dodge");
    private readonly int guradHash = Animator.StringToHash("isGuard");
    [SerializeField] private float buffTimer = 0f;
    [SerializeField] private float buffTime;
    public override void Enter()
	{
		stateMachine.Rigidbody.velocity = Vector3.zero;
        stateMachine.Animator.ResetTrigger(attackHash);
        stateMachine.Animator.ResetTrigger(idleHash);
        stateMachine.Animator.SetBool(BuffHash, true);
        buffTimer = 0f;

        stateMachine.InputReader.onLAttackStart += Attack;
        stateMachine.InputReader.onRAttackStart += Gurad;
        stateMachine.InputReader.onJumpStart += Dodge;
    }
	public override void Tick()
	{

        buffTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Mouse1))
//         {
//             stateMachine.Animator.SetBool(guradHash, true);
//         }

        // 특정 조건을 만족할 때 애니메이션을 종료하고 targetStateName으로 전환
        if (buffTimer > buffTime)
        {
            stateMachine.Animator.SetTrigger(idleHash);
        }

        if (PlayerStateMachine.GetInstance().InputReader.moveComposite.magnitude != 0f)
        {
            stateMachine.Animator.SetBool(moveHash, true);
        }

//         if (Input.GetKeyDown(KeyCode.Mouse0))
//         {
//             stateMachine.Animator.SetTrigger(attackHash);
//         }
//         if (Input.GetKeyDown(KeyCode.Space))
//         {
//             stateMachine.Animator.SetTrigger(dodgeHash);
//         }

    }
	public override void FixedTick()
	{
	}
	public override void LateTick()
	{
    }
	public override void Exit()
	{
        stateMachine.InputReader.onLAttackStart -= Attack;
        stateMachine.InputReader.onRAttackStart -= Gurad;
        stateMachine.InputReader.onJumpStart -= Dodge;
        stateMachine.Animator.SetBool(BuffHash, false);
	}
    private void Attack()
    {
        stateMachine.AutoTargetting.AutoTargeting();
        stateMachine.Animator.SetTrigger(attackHash);
    }
    private void Dodge()
    {
        if (stateMachine.Velocity.magnitude != 0f)
        {
            //stateMachine.Animator.SetBool(nextComboHash, false);
            stateMachine.transform.rotation = Quaternion.LookRotation(stateMachine.Velocity);
            stateMachine.Animator.SetTrigger(dodgeHash);
        }
    }
    private void Gurad() { PlayerStateMachine.GetInstance().Animator.SetBool(guradHash, true); }



}
