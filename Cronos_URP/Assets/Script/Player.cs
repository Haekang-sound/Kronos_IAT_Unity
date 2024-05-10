using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;


/// <summary>
/// Player�� ���� ������ �� ���� �� �� �ִ� �÷��̾� ��ũ��Ʈ
/// 1. status
/// 2. Item
/// 3. ���
/// </summary>
public class Player : MonoBehaviour
{
	[Header("State")]
	[SerializeField] private string CurrentState;

	[Header("Move Option")]
	[SerializeField] private float Speed = 5f;
	[SerializeField] private float JumpForce = 10f;
	[SerializeField] private float LookRotationDampFactor = 10f;

	//[Header("Play Option")]
	//[SerializeField] private float HitRange = 5f;

	MeleeWeapon meleeWeapon;

	PlayerStateMachine PlayerFSM;

	public float moveSpeed { get { return Speed; } }
	public float jumpForce { get { return JumpForce; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }

	// chronos in game Option
	private float CP { get; set; }
	private float TP { get; set; }

	// �÷��̾� �����͸� �����ϰ� respawn�� �ݿ��ϴ� ������
	PlayerData playerData = new PlayerData();
	Transform playerTransform;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	private void Start()
	{
		// ����/���� �����Լ��� �ӽ÷� ����غ���
		// �ݵ�� ���������� �κ������� �ӽ÷� �ִ´�
		PlayerFSM = GetComponent<PlayerStateMachine>();
		playerTransform = GetComponent<Transform>();
		meleeWeapon = GetComponentInChildren<MeleeWeapon>();
		meleeWeapon.SetOwner(gameObject);

	}

	private void Update()
	{

		CurrentState = PlayerFSM.GetState().GetType().Name;

		if (Input.GetKeyDown(KeyCode.I))
		{
			Debug.Log($"����� ������ {playerData.RespawnPos.x}, {playerData.RespawnPos.y}, {playerData.RespawnPos.z}");
		}
	}



	public void SavePlayerData()
	{
		playerData.saveScene = SceneManager.GetActiveScene().name; // ���� ���� �̸��� �����´�
		playerData.TP = TP;
		playerData.TP = CP;
		playerData.RespawnPos = playerTransform.position;
		// �ʿ��� �����͸� ���� ��� ������
	}

	public void PlayerRespawn()
	{
		if(SceneManager.GetActiveScene().name != playerData.saveScene)
		{
			SceneManager.LoadScene(playerData.saveScene);
		}
		TP = playerData.TP;
		CP = playerData.CP;
		playerTransform.position = playerData.RespawnPos;
	}


	// �÷��̾ ������
	public void PlayerDeath()
	{
		TP = 0f;
	}

	public void AttackStart()
	{
		meleeWeapon.BeginAttack(false);
	}
	public void AttackEnd() 
	{
		meleeWeapon.EndAttack();
	}

	
}
