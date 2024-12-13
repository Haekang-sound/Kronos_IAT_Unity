using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(InputReader))] // ��Ʈ����Ʈ�� ��ӹ��� 
[RequireComponent(typeof(Animator))]    // ������� ��Ʈ����Ʈ RequireComponenet
[RequireComponent(typeof(Rigidbody))]   // �ش�������Ʈ�� �߰����ش�

/// <summary>
/// Player�� ���¸� ���۽�Ű�� Ŭ����
/// 
/// ohk    v1
/// </summary>
public class PlayerStateMachine : StateMachine
{
	private static PlayerStateMachine _instance;
	public static PlayerStateMachine GetInstance() { return _instance; }

	public Vector3 velocity;
	public int currentLayerIndex;
	public AutoTargetting autoTargetting;

	public bool IsRattack { get; set; }
	public bool IsGrounded { get; set; }
	public bool DodgeBool { get; set; }
	public float MoveForce { get; set; }
	public float minf { get; set; }

	public Transform MainCamera { get; private set; }
	public Transform PlayerTransform { get; private set; }
	public AnimatorStateInfo currentStateInformable { get; set; }

	public Player Player { get; private set; }
	public InputReader InputReader { get; private set; }
	public Animator Animator { get; private set; }
	public Rigidbody Rigidbody { get; private set; }
	public HitStop HitStop { get; private set; }
	public GroundChecker GroundChecker { get; private set; }

	public void Awake()
	{
		_instance = this;
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

		// ���� ���¸� �����ش�.
		SwitchState(new PlayerMoveState(this));
	}

	/// <summary>
	/// �÷��̾ �и����·� ��ȯ�Ѵ�.
	/// </summary>
	public void SwitchParryState()
	{
		if (Animator.GetBool("isParry"))
		{
			SwitchState(new PlayerParryState(this));
		}
	}

}
