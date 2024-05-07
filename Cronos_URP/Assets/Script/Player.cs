using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Player�� ���� ������ �� ���� �� �� �ִ� �÷��̾� ��ũ��Ʈ
/// 1. status
/// 2. Item
/// 3. ���
/// </summary>
public class Player : MonoBehaviour
{
	PlayerStateMachine PlayerFSM;
	public string CurrentState;

	[Header("Movement")]
	[SerializeField] private float Speed = 5f;
	[SerializeField] private float JumpForce = 10f;
	[SerializeField] private float LookRotationDampFactor = 10f;

	public float moveSpeed { get { return Speed; }}
	public float jumpForce { get { return JumpForce; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }

	private void Start()
	{
		PlayerFSM = GetComponent<PlayerStateMachine>();
	}

	public void FixedUpdate()
	{
		// ���� ���¸� ǥ���ϱ� ���� ����
		// string���� ������ �� �׷���? 
		// �������� �̽��� �����Ŷ����������� �ʴ´�
		CurrentState = PlayerFSM.GetState().GetType().Name;
	}

	void OnSlashEvent()
	{

	}
}
