using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(InputReader))] // 어트리뷰트를 상속받은 
[RequireComponent(typeof(Animator))]    // 사용지정 어트리뷰트 RequireComponenet
[RequireComponent(typeof(Rigidbody))]	// 해당컴포넌트를 추가해준다

public class PlayerStateMachine : StateMachine
{
	static PlayerStateMachine instance;
	public static PlayerStateMachine GetInstance() { return instance; }

	/// <summary>
	///  이건 우클릭 콤보용
	/// </summary>
	public bool IsRattack { get; set; }
	public Vector3 Velocity;
	public Player Player { get; private set; }
	public InputReader InputReader { get; private set; }
	public Animator Animator { get; private set; }
	public Rigidbody Rigidbody { get; private set; }
	public Transform MainCamera { get; private set; }
	public Transform PlayerTransform { get; private set; }
	public HitStop HitStop { get; private set; }
	public GroundChecker GroundChecker { get; private set; }
	public AutoTargetting AutoTargetting;
	public float MoveForce { get; set; }
	public bool IsGrounded { get; set; }

	public int currentLayerIndex;

	public AnimatorStateInfo currentStateInformable { get; set; }
	public float minf { get; set; }
	public bool DodgeBool { get; set; }

	public void Awake()
	{
		instance = this;
	}
	public void OnEnable()
	{
		Player = GetComponent<Player>();
		InputReader = GetComponent<InputReader>();
		Rigidbody = GetComponent<Rigidbody>();
		Animator = GetComponent<Animator>();

		MainCamera = Camera.main.transform;
		PlayerTransform = GetComponent<Transform>();
		HitStop = GetComponent<HitStop>();
		GroundChecker = GetComponent<GroundChecker>();

		// 시작 상태를 정해준다.
		SwitchState(new PlayerMoveState(this));
	}

	/// <summary>
	/// 플레이어를 패링상태로 전환한다.
	/// </summary>
	public void SwitchParryState()
	{
		if (Animator.GetBool("isParry"))
		{
			SwitchState(new PlayerParryState(this));
		}
	}

}
