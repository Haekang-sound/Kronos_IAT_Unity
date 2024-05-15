using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Purchasing;
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

	public float stopTiming = 0.2f;


    float totalspeed;
    MeleeWeapon meleeWeapon;
    PlayerStateMachine PlayerFSM;
	

    public float moveSpeed { get { return totalspeed; } }
    public float jumpForce { get { return JumpForce; } }
    public float lookRotationDampFactor { get { return LookRotationDampFactor; } }

	// chronos in game Option
	public float CP { get; set; } = 100f;
	public float TP { get; set; } = 100f;

    // �÷��̾� �����͸� �����ϰ� respawn�� �ݿ��ϴ� ������
    PlayerData playerData = new PlayerData();
    Transform playerTransform;
    AutoTargetting targetting;

    private void Awake()
    {
    }

    private void Start()
    {
        // ����/���� �����Լ��� �ӽ÷� ����غ���
        // �ݵ�� ���������� �κ������� �ӽ÷� �ִ´�
        PlayerFSM = GetComponent<PlayerStateMachine>();
        playerTransform = GetComponent<Transform>();
        meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        meleeWeapon.SetOwner(gameObject);
        targetting = GetComponentInChildren<AutoTargetting>();
        totalspeed = Speed;

        if (GameManager.Instance.isRespawn)
        {
            PlayerRespawn();
        }
    }

        private void Update()
    {
        CurrentState = PlayerFSM.GetState().GetType().Name;

        if (Input.GetKeyDown(KeyCode.I))
        {
			StartPlayer();
			Debug.Log($"����� ������ {playerData.RespawnPos.x}, {playerData.RespawnPos.y}, {playerData.RespawnPos.z}");
        }
    }

    public void StartPlayer()
    {
        Start();
        PlayerFSM.Start();
        targetting.Start();
		gameObject.transform.position = new Vector3(0f, 7f, 0f);
    }

    public void SetSpeed(float value)
    {
        totalspeed = value * Speed;
    }


    // �����������͸� GameManager�� ��������
    public void SavePlayerData()
    {
        playerData.saveScene = SceneManager.GetActiveScene().name; // ���� ���� �̸��� �����´�
        playerData.TP = TP;
        playerData.TP = CP;
        playerData.RespawnPos = playerTransform.position;
		// �ʿ��� �����͸� ���� ��� ������
		GameManager.Instance.PlayerDT = playerData;
	}

    
    public void PlayerRespawn()
    {
        if (SceneManager.GetActiveScene().name != GameManager.Instance.PlayerDT.saveScene)
        {
            SceneManager.LoadScene(GameManager.Instance.PlayerDT.saveScene);
        }
        TP = GameManager.Instance.PlayerDT.TP;
        CP = GameManager.Instance.PlayerDT.CP;
		if(GameManager.Instance.PlayerDT.RespawnPos.x == 0f 
			&& GameManager.Instance.PlayerDT.RespawnPos.y == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.z == 0f)
		{
			GameManager.Instance.PlayerDT.RespawnPos = new Vector3(0f, 7f, 0f);
		}
        playerTransform.position = (Vector3)GameManager.Instance.PlayerDT.RespawnPos;
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
