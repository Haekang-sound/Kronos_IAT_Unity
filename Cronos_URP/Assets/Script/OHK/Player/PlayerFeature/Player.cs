using Message;
using System;
using System.Collections;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static ScreenFader;



/// <summary>
/// Player가 갖는 정보를 한 눈에 볼 수 있는 플레이어 스크립트
/// 1. status
/// 2. Item
/// 3. 등등
/// </summary>
public class Player : MonoBehaviour, IMessageReceiver
{
	// 싱글턴 객체 입니다. 
	public static Player Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}

			// 인스턴스가 없다면 계층 구조창에서 검색해서 가져옴.
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
	/// floating capsule만드는중 
	/// </summary>
	[field: Header("Collisions")]
	[field: SerializeField] public CapsuleColldierUtility CapsuleColldierUtility { get; private set; }
	[field: SerializeField] public PlayerLayerData LayerData { get; private set; }
	[field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }


	private Checkpoint _currentCheckpoint;


	// 감지하고자 하는 레이어를 지정합니다.
    public LayerMask targetLayer; // Inspector에서 설정 가능

	// Property
	private float totalspeed;
	public float moveSpeed { get { return totalspeed; } }
	public float lookRotationDampFactor { get { return LookRotationDampFactor; } }
	public float AttackCoefficient { get { return attackCoefficient; } set { attackCoefficient = value; } }
	public float MoveCoefficient { get { return moveCoefficient; } set { moveCoefficient = value; } }

	//     // 스킬 사용정보
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
	public bool IsEnforced { get { return isEnforced; } set { isEnforced = value; } }   // 강화상태를 위한 프로퍼티
	public bool IsLockOn { get { return isLockOn; } set { isLockOn = value; } }
	public bool RigidImmunity { get { return rigidImmunity; } set { rigidImmunity = value; } }
	public bool DodgeAttack { get { return dodgeAttack; } set { dodgeAttack = value; } }

	// 플레이어 데이터를 저장하고 respawn시 반영하는 데이터
	PlayerData playerData = new PlayerData();
	Transform playerTransform;
	AutoTargetting targetting;

	public MeleeWeapon meleeWeapon;
	ShieldWeapon shieldWeapon;
	PlayerStateMachine PlayerFSM;

	public Damageable _damageable;
	public Defensible _defnsible;
	private KnockBack _knockBack;

	public SoundManager soundManager;
	public EffectManager effectManager;
	public ImpulseCam impulseCam;
	public GameObject playerSword;
	public GameObject spcCubeL;
	public GameObject spcCubeR;
	public float spcDelay;
	public PlayerStateMachine psm;
	public BoxColliderAdjuster adjuster;

	public bool useKnockback;
	public bool isParry;
	public bool isDecreaseTP;

	//public float spcActivateTime;


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
}
	private void OnValidate()
	{
		CapsuleColldierUtility.Initialize(gameObject);
		CapsuleColldierUtility.CalculateCapsuleColliderDimensions();

	}
	protected void OnDisable()
	{
		_damageable.onDamageMessageReceivers.Remove(this);
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
		Cursor.visible = false;
		// TEST: 데이터 로드 
		Load();

		// 여기에 초기화
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

		// 문제해결을 위해 옮김 
		meleeWeapon.SetOwner(gameObject);
		
		// simpleDamager없어지지 않았나?
		meleeWeapon.simpleDamager.damageAmount = currentDamage;
		meleeWeapon.parryDamaer.damageAmount = currentDamage;

		meleeWeapon.parryDamaer.parrying.AddListener(ChangeParryState);
		


		totalspeed = Speed;
		_damageable.maxHitPoints = maxTP;
		_damageable.currentHitPoints = currentTP;
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
		// 액티브스킬이라면 cp를 채우지 않는다.
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
	// 타격시 tp흡수량
	public float TPGain()
	{
		return TPAbsorptionRatio /** currentDamage*/;
	}

	public float currentTime = 0f;
	bool timeSlash;
	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.H))
 		{
			transform.position += transform.forward*5f;
 		}
// 
// 		if(timeSlash)
// 		{
// 			currentTime += Time.deltaTime;
// 			PlayerFSM.Rigidbody.velocity = transform.forward*TimeSlashCurve.Evaluate(currentTime);
// 			if(currentTime > 1f)
// 			{
// 				timeSlash = false;
// 				currentTime = 0f;
// 			}
// 		}
// 		else
// 		{
// 			currentTime = 0f;
// 		}


		if (Input.GetKeyDown(KeyCode.O))
		{	
			BulletTime.Instance.SetNormalSpeed();
			//Time.timeScale = 1f;
		}

		// 버프중이라면
		if (isBuff)
		{
			buffTimer += Time.deltaTime;
		}
		else // 아니라면
		{
			buffTimer = 0f;
		}

		// 특정 조건을 만족할 때 애니메이션을 종료하고 targetStateName으로 전환
		if (buffTimer > buffTime)
		{
			//PlayerFSM.Animator.SetBool("combMove",false);
			PlayerFSM.Animator.SetBool("isEnforced",false);
			effectManager.SwordAuraOff(); 
		}

		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			SetCursorInactive();
		}

		// 능력개방치트
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			PlayerFSM.Animator.SetBool("isCPBoomb", true);
			PlayerFSM.Animator.SetBool("isTimeStop", true);
			PlayerFSM.Animator.SetBool("ComAttackVariation", true);
			PlayerFSM.Animator.SetBool("NorAttackVariation", true);
			PlayerFSM.Animator.SetBool("DodgeAttack", true);
			PlayerFSM.Animator.SetBool("EnforcedCombo", true);
		}
		if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			PlayerFSM.Animator.SetBool("isCPBoomb", false);
			PlayerFSM.Animator.SetBool("isTimeStop", false);
			PlayerFSM.Animator.SetBool("ComAttackVariation", false);
			PlayerFSM.Animator.SetBool("NorAttackVariation", false);
			PlayerFSM.Animator.SetBool("DodgeAttack", false);
			PlayerFSM.Animator.SetBool("EnforcedCombo", false);
		}

		CurrentState = PlayerFSM.GetState().GetType().Name;

		// 실시간으로 TP 감소
		if (_damageable.currentHitPoints > 0f)
		{
			_damageable.currentHitPoints -= Time.deltaTime;

            if (TP <= 0)
            {
                _damageable.JustDead();
            }
        }

		// 실시간으로 CP감소
		if (IsDecreaseCP && CP > 0)
		{
			CP -= Time.deltaTime * CPDecayRatio;
			if (CP <= 0)
			{
				TimeNormalization();
			}
		}

		TP = _damageable.currentHitPoints;

		// 움직일 때마다 spc큐브를 활성화.
		if (psm.Velocity.x != 0f || psm.Velocity.z != 0f)
		{
			StartCoroutine(ActivateSpcCubes(spcDelay));
		}
		else
		{
			spcCubeL.SetActive(false);
			spcCubeR.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		meleeWeapon.parryDamaer.parrying.RemoveListener(ChangeParryState);
	}

	public void OnReceiveMessage(MessageType type, object sender, object data)
	{
		switch (type)
		{
			case MessageType.DAMAGED:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;

					Damaged(damageData);
				}
				break;
			case MessageType.DEAD:
				{
					Damageable.DamageMessage damageData = (Damageable.DamageMessage)data;
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
	void Damaged(Damageable.DamageMessage damageMessage)
	{
		// 여기서 리턴하면 애니메이션만 재생하지 않는다.
		// 
		if (//PlayerFSM.GetState().ToString() == "PlayerDefenceState" ||
			PlayerFSM.GetState().ToString() == "PlayerDodgeState" ||
			PlayerFSM.GetState().ToString() == "PlayerDamagedState" ||
			rigidImmunity || isEnforced)
		{
			return;
		}

		if (useKnockback)
		{
			_knockBack?.Begin(damageMessage.damageSource);
		}

		PlayerFSM.Animator.SetTrigger("Damaged");
	}

	// 죽었을 때 호출되는 함수
	public void Death(/*Damageable.DamageMessage msg*/)
	{
        StartCoroutine(DeathScequence());
	}

	private IEnumerator DeathScequence()
	{
		yield return ScreenFader.FadeSceneOut(FadeType.GameOver);

		while (ScreenFader.IsFading)
		{
			yield return null;
		}

        yield return new WaitForSecondsRealtime(3);

		//PauseManager.Instance.PauseGame();

        yield return ScreenFader.FadeSceneIn(FadeType.GameOver);

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        yield return ScreenFader.FadeSceneOut(FadeType.Black);


        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        //Respawn();
        if (_currentCheckpoint != null)
        {
			//GetComponent<Rigidbody>().enable
			PlayerFSM.Rigidbody.position = _currentCheckpoint.transform.position;
			PlayerFSM.Rigidbody.rotation = _currentCheckpoint.transform.rotation;
			TP = _currentCheckpoint.healTP;
			CP = _currentCheckpoint.healCP;

//             transform.position = _currentCheckpoint.transform.position;
//             transform.rotation = _currentCheckpoint.transform.rotation;
		}
        else
        {
            Debug.LogError("체크포인트가 없는 데스");
        }

        //PauseManager.Instance.UnPauseGame();

        yield return ScreenFader.FadeSceneIn(FadeType.Black);

    }

	/// <summary>
	/// 감속상태 정상화
	/// </summary>
	public void TimeNormalization()
	{
		IsDecreaseCP = false;
		CP = 0;
		SkillRenderObj.SetActive(false);
		BulletTime.Instance.SetNormalSpeed();
	}

	public void SetCursorInactive()
	{
		Cursor.visible = !Cursor.visible; // 마우스 안보이게 하기
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
		playerData.saveScene = SceneManager.GetActiveScene().name; // 현재 씬의 이름을 가져온다
		playerData.TP = TP;
		playerData.TP = CP;
		playerData.RespawnPos = playerTransform.position;
	}

	// 플레이어를 죽이자
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
	/// 시간정지 관련함수
	/// </summary>

	public void TimeStop()
	{
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
	///  시간정지 효과를 멈추는 함수 수정할것
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


	// 기본 슬래시 FX
	// 이름이 망해부렀으야
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

	public void Shake()
	{
		if (impulseCam != null)
			impulseCam.Shake();
	}

	public void SpeedLine()
	{
		if (effectManager != null)
			effectManager.SpeedLine();
	}

	IEnumerator ActivateSpcCubes(float delay)
	{
		yield return new WaitForSeconds(delay);
		if (psm.Velocity.x != 0f || psm.Velocity.z != 0f)
		{
			spcCubeL.SetActive(true);
			spcCubeR.SetActive(true);
		}
	}

	// 플레이어 spcCube를 활성화 - 키프레임에서 이벤트로 호출
	public void ActivateSCube()
	{
		spcCubeL.SetActive(true);
		spcCubeR.SetActive(true);
	}

	public void DeactivateSCube()
	{
		if (spcCubeL.activeSelf)
			spcCubeL.SetActive(false);
		if (spcCubeR.activeSelf)
			spcCubeR.SetActive(false);
	}

	public void SetCheckpoint(Checkpoint checkpoint)
	{
		if (_currentCheckpoint != null)
		{
			if (_currentCheckpoint.priority > checkpoint.priority)
			{
				return;
			}
		}

		_currentCheckpoint = checkpoint;
        TP += checkpoint.healTP;
	}

	public void TempRespawn()
	{


	}

	public void Respawn()
	{
        if (_currentCheckpoint != null)
        {
            transform.position = _currentCheckpoint.transform.position;
            transform.rotation = _currentCheckpoint.transform.rotation;
			TP = _currentCheckpoint.healTP;
			CP = _currentCheckpoint.healCP;
        }
        else
        {
            Debug.LogError("체크포인트가 없는 데스");
			TP = maxTP;
			// CP 초기화
			currentCP = 0f;
		}


        
    }

	protected IEnumerator RespawnRoutine()
	{
		// 1초 동안 페이드 아웃.
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
		PlayerPrefs.SetFloat("maxTP", maxTP);
		PlayerPrefs.SetFloat("maxCP", maxCP);

		PlayerPrefs.SetFloat("currentTP", currentTP);
		PlayerPrefs.SetFloat("currentCP", currentCP);
	}
	
	internal void Load()
	{
		if (PlayerPrefs.HasKey("maxTP"))
		{
			maxTP = PlayerPrefs.GetFloat("maxTP");
			maxCP = PlayerPrefs.GetFloat("maxCP");
			currentTP = PlayerPrefs.GetFloat("currentTP");
			currentCP = PlayerPrefs.GetFloat("currentCP");

		}
		//else
		//{
		//   Debug.Log("Player Load faile");
		//}
	}
}
