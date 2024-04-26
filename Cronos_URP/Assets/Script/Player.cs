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

	[Header("State")]
	[SerializeField] private string CurrentState;
	[SerializeField] private string Attribute;

	[Header("Move Option")]
	[SerializeField] private float Speed = 5f;
	[SerializeField] private float JumpForce = 10f;
	[SerializeField] private float LookRotationDampFactor = 10f;

	public float moveSpeed { get { return Speed; } }
	public float jumpForce { get { return JumpForce; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }

	// chronos in game Option
	private float CP { get; set; }
	private float TP { get; set; }


	bool isAccel = false;

	private void Start()
	{
		PlayerFSM = GetComponent<PlayerStateMachine>();

		// ����/���� �����Լ��� �ӽ÷� ����غ���
		// �ݵ�� ���������� �κ������� �ӽ÷� �ִ´�
		Attribute = "Is Noting";
		PlayerFSM.InputReader.onSwitching += Switching;
	}

	public void FixedUpdate()
	{
		// ���� ���¸� ǥ���ϱ� ���� ����
		// string���� ������ �� �׷���? 
		// �������� �̽��� �����Ŷ����������� �ʴ´�
		CurrentState = PlayerFSM.GetState().GetType().Name;
	}

	// ����, ���� ��ȭ�� ���� �ӽ��Լ� 
	public void Switching()
	{
		isAccel = !isAccel;

		if (isAccel)
		{
			Attribute = "����";
		}
		else
		{
			Attribute = "����";
		}

	}



}
