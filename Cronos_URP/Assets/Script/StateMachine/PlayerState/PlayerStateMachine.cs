using UnityEditor.Tilemaps;
using UnityEditorInternal;

using UnityEngine;
[RequireComponent(typeof(InputReader))]         // ��Ʈ����Ʈ�� ��ӹ��� 
[RequireComponent(typeof(Animator))]            // ������� ��Ʈ����Ʈ RequireComponenet
[RequireComponent(typeof(CharacterController))] // �ش�������Ʈ�� �߰����ش�
[RequireComponent(typeof(Player))]
public class PlayerStateMachine : StateMachine
{
	public Vector3 Velocity;
	public Player Player { get; private set; }
	public Transform MainCamera { get; private set; }
	public InputReader InputReader { get; private set; }
	public Animator Animator { get; private set; }
	public CharacterController Controller { get; private set; }
	public Transform PlayerTransform { get; private set; }

	// FX�� ���� �ӽ� property
	public EffectManager EffectManager { get; private set; }
	public HitStop HitStop { get; private set; }

	private void Start()
	{
		MainCamera = Camera.main.transform;

		Player = GetComponent<Player>();

		InputReader = GetComponent<InputReader>();
		Animator = GetComponent<Animator>();

		Controller = GetComponent<CharacterController>();
		PlayerTransform = GetComponent<Transform>();
		EffectManager = GameObject.Find("EffectManager").GetComponent<EffectManager>();
		HitStop = GetComponent<HitStop>();

		// ���� ���¸� �����ش�.
		//SwitchState(new PlayerMoveState(this));
		SwitchState(new PlayerIdleState(this));
	}
	void OnSlashEvent()
	{
		EffectManager.PlayerSlash();
	}

}
