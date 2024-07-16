
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

		// ���� ���¸� �����ش�.
		SwitchState(new PlayerIdleState(this));
	}


	public void AnimationStop()
	{
		Animator.speed = 0f; 
	}

	public void AnimationGO()
	{
		Animator.speed = 1f;
	}

}
