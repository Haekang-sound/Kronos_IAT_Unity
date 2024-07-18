
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
//using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(InputReader))]         // ��Ʈ����Ʈ�� ��ӹ��� 
[RequireComponent(typeof(Animator))]            // ������� ��Ʈ����Ʈ RequireComponenet
[RequireComponent(typeof(Rigidbody))] // �ش�������Ʈ�� �߰����ش�

public class PlayerStateMachine : StateMachine
{
	static PlayerStateMachine instance;
	public static PlayerStateMachine GetInstance() { return instance; }

	public Vector3 Velocity;
    public Player Player { get; private set; }
    public InputReader InputReader { get; private set; }
    public Animator Animator { get; private set; }
	public Rigidbody Rigidbody { get; private set; }
    public Transform MainCamera { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public HitStop HitStop { get; private set; }
	public AutoTargetting AutoTargetting;
	public GroundChecker GroundChecker;
	public float MoveForce {  get; set; }
	public bool IsGrounded {  get;  set; }

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

		// ���� ���¸� �����ش�.
		SwitchState(new PlayerIdleState(this));
	}

	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float radius = 0.3f;
	[SerializeField] private float offset = 0.1f;
	[SerializeField] private bool drawGizmo;
}
