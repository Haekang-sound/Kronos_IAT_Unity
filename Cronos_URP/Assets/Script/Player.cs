using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.PlayerLoop;
using UnityEngine.Purchasing;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;


/// <summary>
/// Player�� ���� ������ �� ���� �� �� �ִ� �÷��̾� ��ũ��Ʈ
/// 1. status
/// 2. Item
/// 3. ���
/// </summary>
public class Player : MonoBehaviour, IMessageReceiver
{
	public static readonly int hashDamageBase = Animator.StringToHash("hit01");

	[Header("State")]
	[SerializeField] private string CurrentState;

	[Header("Move Option")]
	[SerializeField] private float Speed = 5f;
	/*[SerializeField] */private float JumpForce = 10f; // ���� ����� ������
	[SerializeField] private float LookRotationDampFactor = 10f;
	//[SerializeField] private float Damage;
	[SerializeField] private float attackCoefficient = 0.1f;
	[SerializeField] private float moveCoefficient = 0.1f;

	public float stopTiming = 0.2f; // ��Ʈ��ž�� �̷������� �۵��ϴ°� �ƴҰ��̴�. ��ž �ð������� �ٲ㼭 ���°� ������
	[SerializeField] private float maxTP;
	[SerializeField] private float maxCP;

	[SerializeField] private float currentDamage;

	[SerializeField] private float currentTP;
	[SerializeField] private float currentCP;
	[SerializeField] private float chargingCP = 10f;


	// Property
	private float totalspeed;
	public float moveSpeed { get { return totalspeed; } }
	public float jumpForce { get { return JumpForce; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }
	public float AttackCoefficient { get { return attackCoefficient; } set { attackCoefficient = value; } }
	public float MoveCoefficient { get { return moveCoefficient; } set { moveCoefficient = value; } }
	// chronos in game Option
	public float MaxCP { get { return maxCP; } set { maxCP = value; } }
	public float MaxTP { get { return maxTP; } set { maxTP = value; } }// => currentTP = value; }
	public float CP { get { return currentCP; } set { currentCP = value; } }
	public float TP { get { return currentTP; } set { currentTP = value; } }// => currentTP = value; }
	public float CurrentDamage { get { return currentDamage; } set { currentDamage = value; } }// => currentTP = value; }
	public bool IsDecreaseCP { get; set; }

	// �÷��̾� �����͸� �����ϰ� respawn�� �ݿ��ϴ� ������
	PlayerData playerData = new PlayerData();
	Transform playerTransform;
	AutoTargetting targetting;

	MeleeTriggerEnterDamager meleeWeapon;
	PlayerStateMachine PlayerFSM;

	public Damageable _damageable;
	public Defensible _defnsible;

	private void Awake()
	{
	}

	protected void OnDisable()
	{
		_damageable.onDamageMessageReceivers.Remove(this);
	}

	private void OnEnable()
	{
		_damageable = GetComponent<Damageable>();
		_defnsible = GetComponent<Defensible>();
		_damageable.onDamageMessageReceivers.Add(this);

		// ����/���� �����Լ��� �ӽ÷� ����غ���
		// �ݵ�� ���������� �κ������� �ӽ÷� �ִ´�
		PlayerFSM = GetComponent<PlayerStateMachine>();
		playerTransform = GetComponent<Transform>();

		meleeWeapon = GetComponentInChildren<MeleeTriggerEnterDamager>();
		meleeWeapon.SetOwner(gameObject);
		meleeWeapon.OnTriggerEnterEvent += ChargeCP;

		targetting = GetComponentInChildren<AutoTargetting>();
		totalspeed = Speed;

		if (GameManager.Instance.isRespawn)
		{
			PlayerRespawn();
		}

		//AdjustAttackPower(Damage);  // ������ ����
		_damageable.currentHitPoints += maxTP;

		GameManager.Instance.PlayerDT = playerData;
		GameManager.Instance.PlayerDT.saveScene = SceneManager.GetActiveScene().name;

	}
	private void ChargeCP(Collider other)
	{
		if (other.CompareTag("Respawn"))
		{
			Debug.Log("cp�� ȸ���Ѵ�.");
			if (CP < maxCP)
			{
				CP += chargingCP;

				if (CP > maxCP) 
				{
					CP = maxCP;
				}
			}
		}
	}

	private void Update()
	{
		CurrentState = PlayerFSM.GetState().GetType().Name;

		// �ǽð����� TP ����
		_damageable.currentHitPoints -= Time.deltaTime;
		// �ǽð����� CP����
		if (IsDecreaseCP && CP > 0)
		{
			CP -= Time.deltaTime;
			if (CP <= 0)
			{
				IsDecreaseCP = false;
				CP = 0;
				Debug.Log("���͵��� �ӵ��� ������� ���ƿ´�.");
				BulletTime.Instance.SetNormalSpeed();
			}
		}


		TP = _damageable.currentHitPoints;

		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (currentTP + 100f < maxTP)
			{
				currentTP = currentTP + 100;
				_damageable.currentHitPoints = currentTP;
			}
			else
			{
				currentTP = maxTP;
				_damageable.currentHitPoints = currentTP;
			}

		}

		if (TP <= 0)
		{
			_damageable.JustDead();
		}

		currentDamage = meleeWeapon.damageAmount;

	}

	private void LateUpdate()
	{

	}

	private void OnTriggerEnter(Collider other)
	{

	}


	public void AdjustTP(float value)
	{
		maxTP += value;
		_damageable.currentHitPoints += value;
	}

	public void AdjustAttackPower(float value)
	{
		currentDamage = value;
		meleeWeapon.damageAmount = currentDamage;
	}
	public void AdjustSpeed(float vlaue)
	{
		Speed += vlaue;
	}

	public void OnReceiveMessage(MessageType type, object sender, object data)
	{
		switch (type)
		{
			case MessageType.DAMAGED:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
					if (true)
					{
						damageData.amount = 0;
					}
					Damaged(damageData);
				}
				break;
			case MessageType.DEAD:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
					Death(damageData);
				}
				break;
		}
	}



	void Damaged(Damageable.DamageMessage damageMessage)
	{
		PlayerFSM.Animator.CrossFadeInFixedTime(hashDamageBase, 0.1f);
	}


	public void Death(Damageable.DamageMessage msg)
	{
		PlayerDeadRespawn();
	}


	public void StartPlayer()
	{
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

	public void PlayerDeadRespawn()
	{
		if (SceneManager.GetActiveScene().name != GameManager.Instance.PlayerDT.saveScene)
		{
			SceneManager.LoadScene(GameManager.Instance.PlayerDT.saveScene);
		}
		else
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		TP = maxTP;
		CP = GameManager.Instance.PlayerDT.CP;
		if (GameManager.Instance.PlayerDT.RespawnPos.x == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.y == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.z == 0f)
		{
			GameManager.Instance.PlayerDT.RespawnPos = new Vector3(0f, 7f, 0f);
		}
		else
		{

			playerTransform.position = (Vector3)GameManager.Instance.PlayerDT.RespawnPos;
		}
	}


	public void PlayerRespawn()
	{
		if (SceneManager.GetActiveScene().name != GameManager.Instance.PlayerDT.saveScene)
		{
			SceneManager.LoadScene(GameManager.Instance.PlayerDT.saveScene);
		}
		TP = GameManager.Instance.PlayerDT.TP;
		CP = GameManager.Instance.PlayerDT.CP;
		if (GameManager.Instance.PlayerDT.RespawnPos.x == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.y == 0f
			&& GameManager.Instance.PlayerDT.RespawnPos.z == 0f)
		{
			GameManager.Instance.PlayerDT.RespawnPos = new Vector3(0f, 7f, 0f);
		}
		else
		{
			playerTransform.position = (Vector3)GameManager.Instance.PlayerDT.RespawnPos;
		}
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
