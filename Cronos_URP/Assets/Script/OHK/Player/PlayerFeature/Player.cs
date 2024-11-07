using Message;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using static Damageable;
using static ScreenFader;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// Player�� ���� ������ �� ���� �� �� �ִ� �÷��̾� ��ũ��Ʈ
/// 1. status
/// 2. Item
/// 3. ���
/// </summary>
public class Player : MonoBehaviour, IMessageReceiver
{
	// �̱��� ��ü �Դϴ�. 
	public static Player Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}

			// �ν��Ͻ��� ���ٸ� ���� ����â���� �˻��ؼ� ������.
			instance = FindObjectOfType<Player>();

			return instance;
		}
	}
	protected static Player instance;

	public RenderObjects SkillRenderObj;

	[Header("State")]
	[SerializeField] private string CurrentState;
	[SerializeField] public AnimationCurve TimeSlashCurve;

	[Header("Move Option")]
	[SerializeField] private float Speed = 1f;
	[SerializeField] private float LookRotationDampFactor = 10f;
	[SerializeField] private float attackCoefficient = 0.1f;
	[SerializeField] private float moveCoefficient = 0.1f;

	[SerializeField] private float maxTP;
	[SerializeField] private float maxCP;

	[SerializeField] private float currentDamage;
	[SerializeField] private float attackSpeed;
	[SerializeField] private float chargeAttack = 0f;

	[SerializeField] private float currentTP;
	[SerializeField] private float currentCP;
	[SerializeField] private float chargingCP = 10f;
	[SerializeField] private float TPAbsorptionRatio = 1f;
	[SerializeField] private float CPDecayRatio = 1f;

	[SerializeField] private bool isEnforced = false;
	[SerializeField] private bool isLockOn = false;
	[SerializeField] private bool rigidImmunity = false;
	[SerializeField] private bool dodgeAttack = false;

	public bool isBuff = false;
	public float buffTimer = 3f;
	public float buffTime = 3f;

	/// <summary>
	/// floating capsule������� 
	/// </summary>
	[field: Header("Collisions")]
	[field: SerializeField] public CapsuleColldierUtility CapsuleColldierUtility { get; private set; }
	[field: SerializeField] public PlayerLayerData LayerData { get; private set; }
	[field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }


	// �����ϰ��� �ϴ� ���̾ �����մϴ�.
	public LayerMask targetLayer; // Inspector���� ���� ����
	public float targetdistance = 1f;

	// Property
	private float totalspeed;
	public float moveSpeed { get { return totalspeed; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }
	public float AttackCoefficient { get { return attackCoefficient; } set { attackCoefficient = value; } }
	public float MoveCoefficient { get { return moveCoefficient; } set { moveCoefficient = value; } }

	//     // ��ų �������
	//     public AbilityUsageInfo AbilityUsageInfo { get { return AbilityUsageInfo; } }

	// chronos in game Option
	public float MaxCP { get { return maxCP; } set { maxCP = value; } }
	public float MaxTP { get { return maxTP; } set { maxTP = value; } }
	public float CP { get { return currentCP; } set { currentCP = value; } }
	public float TP
	{
		get { return currentTP; }
		set
		{
			currentTP = value;
			if (currentTP > maxTP)
			{
				maxTP = currentTP;
			}
			_damageable.currentHitPoints = currentTP;
		}
	}
	public float ChargingCP { get { return chargingCP; } set { chargingCP = value; } }
	public float CurrentDamage { get { return currentDamage; } set { currentDamage = value; } }
	public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
	public float ChargeAttack { get { return chargeAttack; } set { chargeAttack = value; } }
	public bool IsDecreaseCP { get; set; }
	public bool IsEnforced { get { return isEnforced; } set { isEnforced = value; } }   // ��ȭ���¸� ���� ������Ƽ
	public bool IsLockOn { get { return isLockOn; } set { isLockOn = value; } }
	public bool RigidImmunity { get { return rigidImmunity; } set { rigidImmunity = value; } }
	public bool DodgeAttack { get { return dodgeAttack; } set { dodgeAttack = value; } }

	// �÷��̾� �����͸� �����ϰ� respawn�� �ݿ��ϴ� ������
	PlayerData playerData = new PlayerData();
	Transform playerTransform;
	AutoTargetting targetting;

	public MeleeWeapon meleeWeapon;
	ShieldWeapon shieldWeapon;
	PlayerStateMachine PlayerFSM;

	public Damageable _damageable;
	public GroggyStack groggyStack;
	public Defensible _defnsible;
	private KnockBack _knockBack;
	private Rigidbody _rigidbody;

	public SoundManager soundManager;
	public EffectManager effectManager;
	public ImpulseCam impulseCam;
	public GameObject playerSword;
	//public GameObject spcCubeL;
	//public GameObject spcCubeR;
	public float spcDelay;
	public PlayerStateMachine psm;
	public BoxColliderAdjuster adjuster;
	//public float spcActivateTime;

	public bool isDecreaseTP = true;
	[SerializeField] private bool usePreiviousSceneData = true;


	private void Awake()
	{
		_knockBack = GetComponent<KnockBack>();
		_defnsible = GetComponent<Defensible>();
		_damageable = GetComponent<Damageable>();
		playerTransform = GetComponent<Transform>();
		PlayerFSM = GetComponent<PlayerStateMachine>();
		meleeWeapon = GetComponentInChildren<MeleeWeapon>();
		shieldWeapon = GetComponentInChildren<ShieldWeapon>();
		targetting = GetComponentInChildren<AutoTargetting>();
		_rigidbody = GetComponent<Rigidbody>();
		groggyStack = GetComponent<GroggyStack>();

	}
	private void OnValidate()
	{
		CapsuleColldierUtility.Initialize(gameObject);
		CapsuleColldierUtility.CalculateCapsuleColliderDimensions();

	}
	protected void OnDisable()
	{
		_damageable.onDamageMessageReceivers.Remove(this);

		// 여기서 능력치를 전부 초기화해준다.

	}

	private void OnEnable()
	{
		_damageable.onDamageMessageReceivers.Add(this);

		CapsuleColldierUtility.Initialize(gameObject);
		CapsuleColldierUtility.CalculateCapsuleColliderDimensions();

		meleeWeapon.parryDamaer.parrying.AddListener(EffectManager.Instance.CreateParryFX);
		meleeWeapon.parryDamaer.parrying.AddListener(PlayerFSM.SwitchParryState);
	}

	void Start()
	{
		// ���⿡ �ʱ�ȭ
		soundManager = SoundManager.Instance;
		effectManager = EffectManager.Instance;
		impulseCam = ImpulseCam.Instance;
		if (soundManager != null)
			Debug.Log("SoundManager found");
		if (effectManager != null)
			Debug.Log("EffectManager found");
		if (impulseCam != null)
			Debug.Log("ImpulseCam found");
		psm = gameObject.GetComponent<PlayerStateMachine>();

		// �����ذ��� ���� �ű� 
		meleeWeapon.SetOwner(gameObject);

		// simpleDamager�������� �ʾҳ�?
		meleeWeapon.simpleDamager.damageAmount = currentDamage;
		meleeWeapon.parryDamaer.damageAmount = currentDamage;

		meleeWeapon.parryDamaer.parrying.AddListener(ChangeParryState);

		totalspeed = Speed;

		_damageable.maxHitPoints = maxTP;
		_damageable.currentHitPoints = currentTP;
		_damageable.OnDeath.AddListener(Death);

		if (usePreiviousSceneData)
			SaveLoadManager.Instance.LoadSceneData();

		groggyStack.OnMaxStack.AddListener(Down);

	}

	private void Down()
	{
		PlayerFSM.Animator.SetTrigger(PlayerHashSet.Instance.down);
        soundManager.PlaySFX("Player_Dead_Sound_SE", transform);
    }

    private void ChangeParryState()
	{
		PlayerFSM.SwitchParryState();
	}

	public void ChargeCP(Collider other)
	{
		{
			if (CP < maxCP && !IsDecreaseCP)
			{
				CP += chargingCP;

				if (CP > maxCP)
				{
					CP = maxCP;
				}
			}
		}
	}
	public void ChargeCP(bool isActiveSkill)
	{
		// ��Ƽ�꽺ų�̶�� cp�� ä���� �ʴ´�.
		if (isActiveSkill)
		{
			return;
		}
		{
			if (CP < maxCP && !IsDecreaseCP)
			{
				CP += chargingCP;

				if (CP > maxCP)
				{
					CP = maxCP;
				}
			}
		}
	}
	public void ChargeCP()
	{
		{
			if (CP < maxCP && !IsDecreaseCP)
			{
				CP += chargingCP;

				if (CP > maxCP)
				{
					CP = maxCP;
				}
			}
		}
	}
	// Ÿ�ݽ� tp�����
	public float TPGain()
	{
		return TPAbsorptionRatio /** currentDamage*/;
	}

	public float currentTime = 0f;
	bool timeSlash;
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			TP += 10f;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			TP -= 10f;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			CP -= 10f;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			CP += 10f;
			if (CP >= 100)
			{
				CP = 100f;
			}
		}

		if (Input.GetKeyDown(KeyCode.H))
		{
			PlayerFSM.Animator.SetTrigger(PlayerHashSet.Instance.down);
		}
		if (Input.GetKeyDown(KeyCode.J))
		{
			PlayerFSM.Animator.SetTrigger(PlayerHashSet.Instance.damagedB);
		}

		// �������̶��
		if (isBuff)
		{
			buffTimer += Time.deltaTime;
		}
		else // �ƴ϶��
		{
			buffTimer = 0f;
		}

		// Ư�� ������ ������ �� �ִϸ��̼��� �����ϰ� targetStateName���� ��ȯ
		if (buffTimer > buffTime)
		{
			isBuff = false;
			PlayerFSM.Animator.SetBool(PlayerHashSet.Instance.isMove, true);
			//PlayerFSM.Animator.SetBool(PlayerHashSet.Instance.isEnforced, false);
// 			if (CurrentState == "PlayerMoveState")
// 			{
				effectManager.SwordAuraOff();

//			}
		}

		CurrentState = PlayerFSM.GetState().GetType().Name;


		// �ǽð����� TP ����
		if (isDecreaseTP && _damageable.currentHitPoints > 0f)
		{
			_damageable.currentHitPoints -= Time.deltaTime;

			if (_damageable.currentHitPoints <= 0)
			{
				_damageable.JustDead();
			}
		}

		// �ǽð����� CP����
		if (IsDecreaseCP && CP > 0)
		{
			CP -= Time.deltaTime * CPDecayRatio;
			if (CP <= 0f)
			{
				TimeNormalization();
			}
		}

		TP = _damageable.currentHitPoints;

	}


	private void OnDestroy()
	{
		meleeWeapon.parryDamaer.parrying.RemoveListener(ChangeParryState);
		ScreenFader.FadeSceneIn(ScreenFader.FadeType.Loading);
	}

	public void OnReceiveMessage(MessageType type, object sender, object data)
	{
		switch (type)
		{
			case MessageType.DAMAGED:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
					Damaged(damageData);
					effectManager.PlayerHitFX(damageData);
				}
				break;
			case MessageType.DEAD:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
					effectManager.PlayerHitFX(damageData);
					PlayerFSM.InputReader.enabled = false;
					//Death(/*damageData*/);
				}
				break;
			case MessageType.RESPAWN:
				{

				}
				break;
		}
	}
	public bool useKnockback;
	public void SetUseKnockback(bool val) => useKnockback = val;
	void Damaged(Damageable.DamageMessage damageMessage)
	{
		// ���⼭ �����ϸ� �ִϸ��̼Ǹ� ������� �ʴ´�.
		// 
		if (PlayerFSM.GetState().ToString() == "PlayerDamagedState")
		{
			if (!PlayerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
			{
				Player.Instance.groggyStack.AddStack();
			}

			return;
		}
		if (PlayerFSM.GetState().ToString() == "PlayerDodgeState" ||
			PlayerFSM.GetState().ToString() == "PlayerDownState" ||
			rigidImmunity || isEnforced)
		{
			return;
		}

		if (useKnockback)
		{
			_knockBack?.Begin(damageMessage.damageSource);
		}

		Vector3 positionToDamager = damageMessage.damageSource - transform.position;
		positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);
		transform.rotation = Quaternion.LookRotation(positionToDamager);


		switch (damageMessage.damageType)
		{

			case DamageType.None:
				PlayerFSM.Animator.SetTrigger(PlayerHashSet.Instance.damagedA);
				if (!PlayerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
					soundManager.PlaySFX("Player_Pain_Sound_SE", transform);
				break;
			case DamageType.ATypeHit:
				PlayerFSM.Animator.SetTrigger(PlayerHashSet.Instance.damagedA);
				if (!PlayerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
					soundManager.PlaySFX("Player_Pain_Sound_SE", transform);
				break;
			case DamageType.BTypeHit:
				PlayerFSM.Animator.SetTrigger(PlayerHashSet.Instance.damagedB);
				if (!PlayerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
					soundManager.PlaySFX("Player_Pain_Sound_SE", transform);
				break;
			case DamageType.Down:
				PlayerFSM.Animator.SetTrigger(PlayerHashSet.Instance.down);
                soundManager.PlaySFX("Player_Dead_Sound_SE", transform);
                break;
		}

	}
	bool isDeath = false;
	// �׾��� �� ȣ��Ǵ� �Լ�
	public void Death(/*Damageable.DamageMessage msg*/)
	{
		if (!isDeath)
		{
			soundManager.PlaySFX("Player_Dead_Sound_SE", transform);
			StartCoroutine(DeathScequence());
			isDeath = true;
		}
	}

	private IEnumerator DeathScequence()
	{
		yield return SceneController.Instance.StartCoroutine(ScreenFader.FadeSceneOut(FadeType.GameOver));

		while (ScreenFader.IsFading)
		{
			yield return null;
		}

		yield return new WaitForSecondsRealtime(3);

		yield return SceneController.Instance.StartCoroutine(ScreenFader.FadeSceneIn(FadeType.GameOver));

		while (ScreenFader.IsFading)
		{
			yield return null;
		}
		//ScreenFader.SetAlpha(0f, FadeType.GameOver);

		yield return SceneController.Instance.StartCoroutine(ScreenFader.FadeSceneOut(FadeType.Black));

		while (ScreenFader.IsFading)
		{
			yield return null;
		}

		SaveLoadManager.Instance.LoadCheckpointData();

		yield return new WaitForSecondsRealtime(3);
		ScreenFader.SetAlpha(0f);
		PlayerFSM.InputReader.enabled = true;
		isDeath = false;
		//yield return SaveLoadManager.Instance.StartCoroutine(ScreenFader.FadeSceneIn(FadeType.Black));
		//while (ScreenFader.IsFading)
		//{
		//    yield return null;
		//}
	}

	/// <summary>
	/// ���ӻ��� ����ȭ
	/// </summary>
	public void TimeNormalization()
	{
		IsDecreaseCP = false;
		CP = 0;
		SkillRenderObj.SetActive(false);
		BulletTime.Instance.SetNormalSpeed();
	}

	void SetCursorInactive()
	{
		Cursor.visible = !Cursor.visible; // ���콺 �Ⱥ��̰� �ϱ�
		if (Cursor.visible)
		{
			Cursor.lockState = CursorLockMode.None;
		}
		else
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	public void SetSpeed(float value)
	{
		totalspeed = value * Speed;
	}

	public void SavePlayerData()
	{
		playerData.saveScene = SceneManager.GetActiveScene().name; // ���� ���� �̸��� �����´�
		playerData.TP = TP;
		playerData.TP = CP;
		playerData.RespawnPos = playerTransform.position;
	}

	// �÷��̾ ������
	public void AttackStart()
	{
		meleeWeapon?.BeginAttack();
	}
	public void AttackEnd()
	{
		meleeWeapon?.EndAttack();
	}

	public void BeginGuard()
	{
		meleeWeapon?.EndAttack();
		shieldWeapon?.BeginGuard();
	}

	public void EndGuard()
	{
		shieldWeapon?.EndGuard();
	}

	public void BeginParry()
	{
		shieldWeapon?.BeginParry();
	}

	public void EndParry()
	{
		shieldWeapon?.EndParry();
	}

	/// <summary>
	/// �ð����� �����Լ�
	/// </summary>

	public void TimeStop()
	{
		soundManager.PlaySFX("TimeStop_Skill_Sound_SE", transform);
		soundManager.PlaySFX("Ult_SE", transform);
		Vector3 temp = transform.position;
		effectManager.SpawnEffect("TeraainScanner", temp);
		SkillRenderObj.SetActive(true);
	}

	public void CPBombGround()
	{
		if (effectManager != null)
		{
			GameObject bomb = effectManager.SpawnEffect("CPGround", transform.position);
			Destroy(bomb, 5.0f);

		}
	}
	public void CPBombSlash()
	{
		if (effectManager != null)
		{
			GameObject slash = effectManager.SpawnEffect("CPSlash", transform.position);

			Destroy(slash, 3.0f);
			Invoke("test", 1.5f);
			Invoke("CPBoombDamager", 1.5f);
			Invoke("TimeNormalization", 1.5f);
		}
	}

	/// <summary>
	///  �ð����� ȿ���� ���ߴ� �Լ� �����Ұ�
	/// </summary>
	public void test()
	{
		SkillRenderObj.SetActive(false);
	}
	public void CPBoombDamager()
	{
		GameObject bombDamager = effectManager.SpawnEffect("CPBombDamager", transform.position);
		Destroy(bombDamager, .5f);
	}

	public void CPBomb()
	{
		if (effectManager != null)
		{
			GameObject bomb = effectManager.SpawnEffect("CPBomb", transform.position);
			Destroy(bomb, 3.0f);


			GameObject bombDamager = effectManager.SpawnEffect("CPBombDamager", transform.position);
			Destroy(bombDamager, 3.0f);
		}
	}

	public void TimeSlash(string name, Vector3 pos)
	{
		if (effectManager != null)
			effectManager.SpawnEffect(name, pos);
	}


	// �⺻ ������ FX
	// �̸��� ���غη�����
	public void NormalSlash()
	{
		if (effectManager != null)
			effectManager.NormalSlashFX("Nor_Attack");
	}

	public void EnforcedSlash()
	{
		if (effectManager != null)
			effectManager.NormalSlashFX("Com_Attack");
	}

	public void DodgeSlash()
	{
		if (effectManager != null)
			effectManager.DodgeAttack();
	}

	public void SwordAura()
	{
		if (effectManager != null)
			effectManager.SwordAuraOn();
	}

	public void NormalStrongSlash()
	{
		if (effectManager != null)
			effectManager.NormalStrongFX();
	}

	public void ComboImpact()
	{
		if (effectManager != null)
		{
			effectManager.GroundCheckFX();
			effectManager.SwordWave();
		}
	}

	public void GroundImpact()
	{
		if (effectManager != null)
			effectManager.GroundCheckFX();
	}

	public void GroundScar()
	{
		if (effectManager != null)
			effectManager.GroundScar("Nor04_Ground");
	}

	public void AbilitySlash()
	{
		if (effectManager != null)
			effectManager.AbilitySlash();
	}

	public void AbilityScar()
	{
		if (effectManager != null)
			effectManager.GroundScar("AbilityGroundScar");
	}

	public void SoundVoice()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Character_voice_SE", transform);
	}

	public void SoundFoot1()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Player_Walk_1_Sound_SE", transform);
	}

	public void SoundFoot2()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Player_Walk_2_Sound_SE", transform);
	}

	// 기획에 전달하기 위해 플레이어 키프레임으로 호출되는 사운드 출력 함수들
	public void ComSound1()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Com_Attack_1_Sound_SE", transform);
	}

	public void ComSound2()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Com_Attack_2_Sound_SE", transform);
	}

	public void ComSound3()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Com_Attack_3_Sound_SE", transform);
	}

	public void ComSound4()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Com_Attack_4_Sound_SE", transform);
	}

	public void NorSound1()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Player_Swing_1_Sound_SE", transform);
	}

	public void NorSound2()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Player_Swing_2_Sound_SE", transform);
	}

	public void DodgeSound()
	{
		if (soundManager != null)
			soundManager.PlaySFX("Player_Dodge_Sound_SE", transform);
	}

	public void Shake()
	{
		if (impulseCam != null)
			impulseCam.Shake();
	}

	//IEnumerator ActivateSpcCubes(float delay)
	//{
	//	yield return new WaitForSeconds(delay);
	//	if (psm.Velocity.x != 0f || psm.Velocity.z != 0f)
	//	{
	//		spcCubeL.SetActive(true);
	//		spcCubeR.SetActive(true);
	//	}
	//}

	//// �÷��̾� spcCube�� Ȱ��ȭ - Ű�����ӿ��� �̺�Ʈ�� ȣ��
	//public void ActivateSCube()
	//{
	//	spcCubeL.SetActive(true);
	//	spcCubeR.SetActive(true);
	//}

	//public void DeactivateSCube()
	//{

	//}

	protected IEnumerator RespawnRoutine()
	{
		// 1�� ���� ���̵� �ƿ�.
		//yield return StartCoroutine(ScreenFader.FadeSceneOut());

		while (ScreenFader.IsFading)
		{
			yield return null;
		}

		// Enable spawning.
		//EllenSpawn spawn = GetComponentInChildren<EllenSpawn>();
		//spawn.enabled = true;

		// If there is a checkpoint, move Ellen to it.

	}

	internal void Save()
	{
		PlayerPrefs.SetFloat("currentTP", currentTP);
	}

	public void ResetAbilityData()
	{
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.EnforcedCombo, false);
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.DodgeAttack, false);
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.isFlashSlash, false);
		EffectManager.Instance.isSwordWave = false;
		EffectManager.Instance.isGroundEnforced = false;
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.isCPBoomb, false);
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.isTimeStop, false);
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.ComAttackVariation, false);
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.NorAttackVariation, false);
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.isParry, false);
		PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.isRushAttack, false);
	}
}
