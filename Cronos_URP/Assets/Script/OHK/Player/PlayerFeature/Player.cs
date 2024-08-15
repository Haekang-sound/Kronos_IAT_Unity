using Message;
using System.Collections;
using UnityEngine;
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

    [Header("State")]
    [SerializeField] private string CurrentState;

    [Header("Move Option")]
    [SerializeField] private float Speed = 5f;
    private float JumpForce = 10f; // 점프 만들면 쓰지뭐
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
    [SerializeField] private float decayTime = 1f;


    [SerializeField] private bool isEnforced = false;
    [SerializeField] private bool isLockOn = false;
    [SerializeField] private bool rigidImmunity = false;
    [SerializeField] private bool dodgeAttack = false;

    private Checkpoint _currentCheckpoint;

    // Property
    private float totalspeed;
    public float moveSpeed { get { return totalspeed; } }
    public float jumpForce { get { return JumpForce; } }
    public float lookRotationDampFactor { get { return LookRotationDampFactor; } }
    public float AttackCoefficient { get { return attackCoefficient; } set { attackCoefficient = value; } }
    public float MoveCoefficient { get { return moveCoefficient; } set { moveCoefficient = value; } }

    // 스킬 사용정보
    public AbilityUsageInfo AbilityUsageInfo { get { return AbilityUsageInfo; } }

    public bool on = false;
    public bool perform = false;
    public bool off = false;
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
                currentTP = maxTP;
            }
            _damageable.CurrentHitPoints = currentTP;
        }
    }
    public float ChargingCP { get { return chargingCP; } set { chargingCP = value; } }
    public float CurrentDamage { get { return currentDamage; } set { currentDamage = value; } }
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    public float ChargeAttack { get { return chargeAttack; } set { chargeAttack = value; } }
    public bool IsDecreaseCP { get; set; }
    public bool IsEnforced { get { return isEnforced; } set { isEnforced = value; } }   // 강화상태를 위한 프로퍼티
    public bool IsLockOn { get { return isLockOn; } set { isLockOn = value; } }
	public bool RigidImmunity { get { return rigidImmunity; } set {  rigidImmunity = value; } }	
	public bool DodgeAttack { get { return dodgeAttack; } set { dodgeAttack = value; } }	

	// 플레이어 데이터를 저장하고 respawn시 반영하는 데이터
	PlayerData playerData = new PlayerData();
    Transform playerTransform;
    AutoTargetting targetting;

    MeleeWeapon meleeWeapon;
    ShieldWeapon shieldWeapon;
    PlayerStateMachine PlayerFSM;

    public Damageable _damageable;
    public Defensible _defnsible;

    public SoundManager soundManager;
    public EffectManager effectManager;
    public ImpulseCam impulseCam;
    public GameObject playerSword;

    protected void OnDisable()
    {
        _damageable.onDamageMessageReceivers.Remove(this);
    }

    private void OnEnable()
    {
        _damageable = GetComponent<Damageable>();
        _damageable.onDamageMessageReceivers.Add(this);

        _defnsible = GetComponent<Defensible>();

        // 감속/가속 변경함수를 임시로 사용해보자
        // 반드시 지워져야할 부분이지만 임시로 넣는다
        PlayerFSM = GetComponent<PlayerStateMachine>();
        playerTransform = GetComponent<Transform>();

        meleeWeapon = GetComponentInChildren<MeleeWeapon>();

        shieldWeapon = GetComponentInChildren<ShieldWeapon>();

        targetting = GetComponentInChildren<AutoTargetting>();
        


        
    }

    void Start()
    {
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


		// 문제해결을 위해 옮김 
		meleeWeapon.simpleDamager.OnTriggerEnterEvent += ChargeCP;
		totalspeed = Speed;
		_damageable.hitPoints = maxTP;
		_damageable.CurrentHitPoints = maxTP;
		meleeWeapon.simpleDamager.damageAmount = currentDamage;
	}

    private void ChargeCP(Collider other)
    {
        {
            Debug.Log("cp를 회복한다.");
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SetCursorInactive();
        }

        CurrentState = PlayerFSM.GetState().GetType().Name;

        // 실시간으로 TP 감소
        if (_damageable.CurrentHitPoints > 0f)
        {
            _damageable.CurrentHitPoints -= Time.deltaTime;
        }

        // 실시간으로 CP감소
        if (IsDecreaseCP && CP > 0)
        {
            CP -= Time.deltaTime * decayTime;
            if (CP <= 0)
            {
                IsDecreaseCP = false;
                CP = 0;
                Debug.Log("몬스터들의 속도가 원래대로 돌아온다.");
                BulletTime.Instance.SetNormalSpeed();
            }
        }

        TP = _damageable.CurrentHitPoints;


        if (TP <= 0)
        {
            _damageable.JustDead();
        }
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
                    Death(/*damageData*/);
                }
                break;
            case MessageType.RESPAWN:
                {

                }
                break;
        }
    }

    void Damaged(Damageable.DamageMessage damageMessage)
    {
        // 여기서 리턴하면 애니메이션만 재생하지 않는다.
		// 
        if (PlayerFSM.GetState().ToString() == "PlayerDefenceState" ||
			PlayerFSM.GetState().ToString() == "PlayerParryState"	||
			PlayerFSM.GetState().ToString() == "PlayerDamagedState" || 
			rigidImmunity || isEnforced)
		{
			return;
		}

		PlayerFSM.Animator.SetTrigger("Damaged");
    }

    // 죽었을 때 호출되는 함수
    public void Death(/*Damageable.DamageMessage msg*/)
    {
        //StartCoroutine(DeathScequence());
    }

    private IEnumerator DeathScequence()
    {
        yield return ScreenFader.FadeSceneOut(FadeType.GameOver);

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        yield return new WaitForSeconds(3);

        yield return ScreenFader.FadeSceneOut(FadeType.Black);

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        Respawn();

        yield return ScreenFader.FadeSceneIn(FadeType.GameOver);

        while (ScreenFader.IsFading)
        {
            yield return null;
        }

        yield return ScreenFader.FadeSceneIn(FadeType.Black);
    }

    void SetCursorInactive()
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

    // 기본 슬래시 FX
    // 이름이 망해부렀으야
    public void SoundSword()
    {
        if (effectManager != null)
            effectManager.NormalSlashFX("Nor_Attack");
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
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
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
        if (_currentCheckpoint != null)
        {
            transform.position = _currentCheckpoint.transform.position;
            transform.rotation = _currentCheckpoint.transform.rotation;
        }
        else
        {
            Debug.LogError("체크포인트가 없는 데스");
        }

        // 1초 동안 페이드 아웃
        //yield return StartCoroutine(ScreenFader.FadeSceneIn());

        /// TODO - 오해강: 초기화 함수를 따로 만들 것

        // TP 초기화 - 적용안됨
        _damageable.ResetDamage();
        TP = maxTP;
        // CP 초기화
        currentCP = 0f;
    }
}
