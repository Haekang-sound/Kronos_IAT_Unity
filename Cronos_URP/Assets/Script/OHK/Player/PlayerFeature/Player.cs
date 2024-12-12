using Message;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.SceneManagement;
using static Damageable;
using static ScreenFader;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 플레이어의 정보를 갖고 있는 클래스
/// 
/// OHK
/// </summary>
public class Player : MonoBehaviour, IMessageReceiver
{
	// 싱글턴을 위한 변수
	public static Player Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}

			instance = FindObjectOfType<Player>();
			return instance;
		}
	}
	protected static Player instance;

	// timestop을 위한 변수
	public RenderObjects skillRenderObj;

	[Header("State")]
	[SerializeField] private string _currentState;

	[Header("Move Option")]
	[SerializeField] private float _speed = 1f;
	[SerializeField] private float _lookRotationDampFactor = 10f;
	[SerializeField] private float _attackCoefficient = 0.1f;
	[SerializeField] private float _moveCoefficient = 0.1f;

	[Header("Status")]
	[SerializeField] private float _maxTP;
	[SerializeField] private float _maxCP;

	[SerializeField] private float _currentTP;
	[SerializeField] private float _currentCP;
	[SerializeField] private float _chargingCP = 10f;
	[SerializeField] private float _TPAbsorptionRatio = 1f;
	[SerializeField] private float _CPDecayRatio = 1f;

	[SerializeField] private float _currentDamage;
	[SerializeField] private float _attackSpeed;
	[SerializeField] private float _chargeAttack = 0f;

	[SerializeField] private bool _isEnforced = false;
	[SerializeField] private bool _isLockOn = false;
	[SerializeField] private bool _isRigidImmunity = false;
	[SerializeField] private bool _isDodgeAttack = false;

	public bool isBuff = false;
	public float buffTimer = 3f;
	public float buffTime = 3f;

	/// <summary>
	/// floating capsule을 위한 프로퍼티
	/// </summary>
	[field: Header("Collisions")]
	[field: SerializeField] public CapsuleColldierUtility capsuleColldierUtility { get; private set; }
	[field: SerializeField] public PlayerLayerData layerData { get; private set; }
	[field: SerializeField] public AnimationCurve slopeSpeedAngles { get; private set; }

	// �����ϰ��� �ϴ� ���̾ �����մϴ�.
	public LayerMask targetLayer;
	public float targetdistance = 1f;
	private float _totalspeed;

	// Property
	public float moveSpeed { get { return _totalspeed; } }
	public float lookRotationDampFactor { get { return _lookRotationDampFactor; } }
	public float AttackCoefficient { get { return _attackCoefficient; } set { _attackCoefficient = value; } }
	public float MoveCoefficient { get { return _moveCoefficient; } set { _moveCoefficient = value; } }

	// chronos in game Option
	public float MaxCP { get { return _maxCP; } set { _maxCP = value; } }
	public float MaxTP { get { return _maxTP; } set { _maxTP = value; } }
	public float CP { get { return _currentCP; } set { _currentCP = value; } }
	public float TP
	{
		get { return _currentTP; }
		set
		{
			_currentTP = value;
			if (_currentTP > _maxTP)
			{
				_maxTP = _currentTP;
			}
			damageable.currentHitPoints = _currentTP;
		}
	}

	public float ChargingCP { get { return _chargingCP; } set { _chargingCP = value; } }
	public float CurrentDamage { get { return _currentDamage; } set { _currentDamage = value; } }
	public float AttackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }
	public float ChargeAttack { get { return _chargeAttack; } set { _chargeAttack = value; } }
	public bool IsDecreaseCP { get; set; }
	public bool IsEnforced { get { return _isEnforced; } set { _isEnforced = value; } }   // ��ȭ���¸� ���� ������Ƽ
	public bool IsLockOn { get { return _isLockOn; } set { _isLockOn = value; } }
	public bool RigidImmunity { get { return _isRigidImmunity; } set { _isRigidImmunity = value; } }
	public bool DodgeAttack { get { return _isDodgeAttack; } set { _isDodgeAttack = value; } }

	// 리스폰을 위한데이터
	PlayerData playerData = new PlayerData();
	Transform playerTransform;

	public MeleeWeapon meleeWeapon;
	public ShieldWeapon shieldWeapon;
	public PlayerStateMachine playerFSM;
	public Damageable damageable;
	public GroggyStack groggyStack;
	public Defensible defnsible;
	public KnockBack knockBack;
	public SoundManager soundManager;
	public EffectManager effectManager;
	public ImpulseCam impulseCam;
	public BoxColliderAdjuster adjuster;

	// 정리해야하는 변수들
	[SerializeField] private bool usePreiviousSceneData = true;
	public GameObject playerSword;
	public bool isDecreaseTP = true;
	public float currentTime = 0f;
	private bool isRelease = false;
	private float releaseLockOn = 0f;
	bool isDeath = false;
	public bool useKnockback;

	private void Awake()
	{
		knockBack = GetComponent<KnockBack>();
		defnsible = GetComponent<Defensible>();
		damageable = GetComponent<Damageable>();
		playerTransform = GetComponent<Transform>();
		playerFSM = GetComponent<PlayerStateMachine>();
		meleeWeapon = GetComponentInChildren<MeleeWeapon>();
		shieldWeapon = GetComponentInChildren<ShieldWeapon>();
		groggyStack = GetComponent<GroggyStack>();

	}
	private void OnValidate()
	{
		capsuleColldierUtility.Initialize(gameObject);
		capsuleColldierUtility.CalculateCapsuleColliderDimensions();

	}
	protected void OnDisable()
	{
		damageable.onDamageMessageReceivers.Remove(this);

		playerFSM.InputReader.onDecelerationStart -= Deceleration;
		playerFSM.InputReader.onLockOnPerformed -= ReleaseLockOn;
		playerFSM.InputReader.onLockOnStart -= LockOn;
		playerFSM.InputReader.onLockOnCanceled -= ReleaseReset;
	}

	private void OnEnable()
	{
		damageable.onDamageMessageReceivers.Add(this);

		capsuleColldierUtility.Initialize(gameObject);
		capsuleColldierUtility.CalculateCapsuleColliderDimensions();

		meleeWeapon.parryDamaer.parrying.AddListener(EffectManager.Instance.CreateParryFX);
		meleeWeapon.parryDamaer.parrying.AddListener(playerFSM.SwitchParryState);
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

		// �����ذ��� ���� �ű� 
		meleeWeapon.SetOwner(gameObject);

		// simpleDamager�������� �ʾҳ�?
		meleeWeapon.simpleDamager.damageAmount = _currentDamage;
		meleeWeapon.parryDamaer.damageAmount = _currentDamage;

		meleeWeapon.parryDamaer.parrying.AddListener(ChangeParryState);

		_totalspeed = _speed;

		damageable.maxHitPoints = _maxTP;
		damageable.currentHitPoints = _currentTP;
		damageable.OnDeath.AddListener(Death);

		if (usePreiviousSceneData)
			SaveLoadManager.Instance.LoadSceneData();

		groggyStack.OnMaxStack.AddListener(Down);

		skillRenderObj.SetActive(false);

		playerFSM.InputReader.onDecelerationStart += Deceleration;
		playerFSM.InputReader.onLockOnPerformed += ReleaseLockOn;
		playerFSM.InputReader.onLockOnStart += LockOn;
		playerFSM.InputReader.onLockOnCanceled += ReleaseReset;

	}

	private void Update()
	{
		// 플레이어의 상태를 받아온다.
		_currentState = playerFSM.GetState().GetType().Name;

		// TP와 CP를 증가, 감소시키는 치트키
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			TP += 10f;
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			TP = 10f;
		}
		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			CP -= 10f;
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			CP = 100f;
		}

		// 마우스 가운데클릭하는 시간이
		// 기준시간을 초과할 경우 
		// 락온을 해제한다.
		if (isRelease)
		{
			releaseLockOn += Time.deltaTime;

			if (releaseLockOn > 1f)
			{
				playerFSM.AutoTargetting.LockOff();
			}
		}

		// 강화상태일 경우 시간을 증가시키고 
		// 강화가 아닐경우 증가시킨 시간을 초기화한다.
		if (isBuff)
		{
			buffTimer += Time.deltaTime;
		}
		else
		{
			buffTimer = 0f;
		}

		// 버프 해제조건을 달성하면 버프를 해제한다.
		if (buffTimer > buffTime
			&& (_currentState == "PlayerBuffState" || _currentState == "PlayerMoveState"))
		{
			isBuff = false;
			playerFSM.Animator.SetBool(PlayerHashSet.Instance.isMove, true);
			_isEnforced = false;
			effectManager.SwordAuraOff();
		}

		// TP감소상태 일 때
		// 현재 TP를 검사하고 0보다 낮아지면 캐릭터를 죽인다.
		if (isDecreaseTP && damageable.currentHitPoints > 0f)
		{
			damageable.currentHitPoints -= Time.deltaTime;

			if (damageable.currentHitPoints <= 0)
			{
				damageable.JustDead();
			}
		}

		// CP감소 상태 일 때
		// CP가 0이하일 경우 감소상태를 해제한다.
		if (IsDecreaseCP && CP > 0)
		{
			CP -= Time.deltaTime * _CPDecayRatio;
			if (CP <= 0f)
			{
				TimeNormalization();
			}
		}
		
		// 캐릭터 TP를 갱신한다.
		TP = damageable.currentHitPoints;
	}

	private void OnDestroy()
	{
		meleeWeapon.parryDamaer.parrying.RemoveListener(ChangeParryState);
		ScreenFader.FadeSceneIn(ScreenFader.FadeType.Loading);
	}

	private void LockOn()
	{
		// 락온 상태가 아니라면
		if (!IsLockOn)
		{
			// 대상을 찾고
			bool temp = IsLockOn = playerFSM.AutoTargetting.FindTarget();
			Debug.Log(temp);
		}
		// 락온상태라면 타겟을 변경한다.
		else
		{
			playerFSM.AutoTargetting.SwitchTarget();
		}
	}
	
	private void Down()
	{
		playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.down);
		soundManager.PlaySFX("Player_Down_Sound_SE", transform);
	}

	private void ChangeParryState()
	{
		playerFSM.SwitchParryState();
	}

	public void ChargeCP(Collider other)
	{
		{
			if (CP < _maxCP && !IsDecreaseCP)
			{
				CP += _chargingCP;

				if (CP > _maxCP)
				{
					CP = _maxCP;
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
			if (CP < _maxCP && !IsDecreaseCP)
			{
				CP += _chargingCP;

				if (CP > _maxCP)
				{
					CP = _maxCP;
				}
			}
		}
	}

	public void ChargeCP()
	{
		{
			if (CP < _maxCP && !IsDecreaseCP)
			{
				CP += _chargingCP;

				if (CP > _maxCP)
				{
					CP = _maxCP;
				}
			}
		}
	}
	
	public float TPGain()
	{
		return _TPAbsorptionRatio /** currentDamage*/;
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
					playerFSM.InputReader.enabled = false;
					playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.Death);
					//Death(/*damageData*/);
				}
				break;
			case MessageType.RESPAWN:
				{

				}
				break;
		}
	}
	
	public void SetUseKnockback(bool val) => useKnockback = val;

	private void Deceleration()
	{
		if (CP >= 100 && playerFSM.Animator.GetBool(PlayerHashSet.Instance.isTimeStop))
		{
			playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.TimeStop);
			BulletTime.Instance.DecelerateSpeed();
			IsDecreaseCP = true;
			damageable.enabled = false;
			BulletTime.Instance.OnActive.Invoke();
		}
		else if (IsDecreaseCP
		 && playerFSM.Animator.GetBool(PlayerHashSet.Instance.isCPBoomb)
		&& !playerFSM.Animator.IsInTransition(playerFSM.currentLayerIndex))
		{
			playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.CPBoomb);
		}
	}

	private void ReleaseLockOn()
	{
		isRelease = true;

		//Debug.Log("누르는중");
		releaseLockOn += Time.deltaTime;

		if (releaseLockOn > 1f)
		{
			playerFSM.AutoTargetting.LockOff();
		}
	}
	private void ReleaseReset()
	{
		isRelease = false;
		releaseLockOn = 0f;
	}


	void Damaged(Damageable.DamageMessage damageMessage)
	{
		// ���⼭ �����ϸ� �ִϸ��̼Ǹ� ������� �ʴ´�.
		// 
		if (playerFSM.GetState().ToString() == "PlayerDamagedState")
		{
			if (!playerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
			{
				Player.Instance.groggyStack.AddStack();
			}

			return;
		}
		if (playerFSM.GetState().ToString() == "PlayerDodgeState" ||
		 playerFSM.GetState().ToString() == "PlayerDownState" ||
		 playerFSM.GetState().ToString() == "PlayerRushAttackState" ||
		 playerFSM.GetState().ToString() == "PlayerDodgeAttackState" ||
		 _isRigidImmunity || _isEnforced)
		{
			return;
		}

		if (useKnockback)
		{
			knockBack?.Begin(damageMessage.damageSource);
		}

		Vector3 positionToDamager = damageMessage.damageSource - transform.position;
		positionToDamager -= transform.up * Vector3.Dot(transform.up, positionToDamager);
		transform.rotation = Quaternion.LookRotation(positionToDamager);


		switch (damageMessage.damageType)
		{

			case DamageType.None:
				playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.damagedA);
				if (!playerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
					soundManager.PlaySFX("Player_Pain_Sound_SE", transform);
				break;
			case DamageType.ATypeHit:
				playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.damagedA);
				if (!playerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
					soundManager.PlaySFX("Player_Pain_Sound_SE", transform);
				break;
			case DamageType.BTypeHit:
				playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.damagedB);
				if (!playerFSM.Animator.GetBool(PlayerHashSet.Instance.isGuard))
					soundManager.PlaySFX("Player_Pain_Sound_SE", transform);
				break;
			case DamageType.Down:
				playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.down);
				soundManager.PlaySFX("Player_Down_Sound_SE", transform);
				break;
		}

	}

	/// 플레이어가 죽었을 경우 동작하는 함수
	public void Death()
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

		yield return SceneController.Instance.StartCoroutine(ScreenFader.FadeSceneOut(FadeType.Black));

		while (ScreenFader.IsFading)
		{
			yield return null;
		}

		SaveLoadManager.Instance.LoadCheckpointData();

		yield return new WaitForSecondsRealtime(3);
		ScreenFader.SetAlpha(0f);
		playerFSM.InputReader.enabled = true;
		playerFSM.Animator.SetTrigger(PlayerHashSet.Instance.Respawn);
		isDeath = false;
	}

	
	/// 시간 정지 스킬을 해제하는 함수
	public void TimeNormalization()
	{
		IsDecreaseCP = false;
		CP = 0;
		skillRenderObj.SetActive(false);
		BulletTime.Instance.SetNormalSpeed();
	}

	public void SetSpeed(float value)
	{
		_totalspeed = value * _speed;
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

	public void TimeStop()
	{
		soundManager.PlaySFX("TimeStop_Skill_Sound_SE", transform);
		soundManager.PlaySFX("Ult_SE", transform);
		Vector3 temp = transform.position;
		effectManager.SpawnEffect("TeraainScanner", temp);
		skillRenderObj.SetActive(true);
	}

	public void CPBombGround()
	{
		if (effectManager != null)
		{
			GameObject bomb = effectManager.SpawnEffect("CPGround", transform.position);
			Destroy(bomb, 5.0f);
			if (soundManager != null)
				soundManager.PlaySFX("TempoExplosion_Sound_SE", transform);
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

	public void test()
	{
		skillRenderObj.SetActive(false);
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
		if (soundManager != null)
		{
			soundManager.PlaySFX("Com_Attack_4_Impact_Sound_SE", transform);
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
		PlayerPrefs.SetFloat("currentTP", _currentTP);
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
