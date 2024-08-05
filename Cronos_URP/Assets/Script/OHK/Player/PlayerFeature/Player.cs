using Message;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ScreenFader;


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

    [Header("State")]
    [SerializeField] private string CurrentState;

    [Header("Move Option")]
    [SerializeField] private float Speed = 5f;
    private float JumpForce = 10f; // ���� ����� ������
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

    private Checkpoint _currentCheckpoint;

    // Property
    private float totalspeed;
    public float moveSpeed { get { return totalspeed; } }
    public float jumpForce { get { return JumpForce; } }
    public float lookRotationDampFactor { get { return LookRotationDampFactor; } }
    public float AttackCoefficient { get { return attackCoefficient; } set { attackCoefficient = value; } }
    public float MoveCoefficient { get { return moveCoefficient; } set { moveCoefficient = value; } }

    // ��ų �������
    public AbilityUsageInfo AbilityUsageInfo { get { return AbilityUsageInfo; } }

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
    public bool IsEnforced { get { return isEnforced; } set { isEnforced = value; } }   // ��ȭ���¸� ���� ������Ƽ
    public bool IsLockOn { get { return isLockOn; } set { isLockOn = value; } }

    // �÷��̾� �����͸� �����ϰ� respawn�� �ݿ��ϴ� ������
    PlayerData playerData = new PlayerData();
    Transform playerTransform;
    AutoTargetting targetting;

    MeleeWeapon meleeWeapon;
    ShieldWeapon shieldWeapon;
    PlayerStateMachine PlayerFSM;

    public Damageable _damageable;
    public Defensible _defnsible;

    SoundManager soundManager;
    EffectManager effectManager;
    ImpulseCam impulseCam;
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

        // ����/���� �����Լ��� �ӽ÷� ����غ���
        // �ݵ�� ���������� �κ������� �ӽ÷� �ִ´�
        PlayerFSM = GetComponent<PlayerStateMachine>();
        playerTransform = GetComponent<Transform>();

        meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        meleeWeapon.simpleDamager.OnTriggerEnterEvent += ChargeCP;

        shieldWeapon = GetComponentInChildren<ShieldWeapon>();

        targetting = GetComponentInChildren<AutoTargetting>();
        totalspeed = Speed;

        _damageable.hitPoints = maxTP;
        _damageable.CurrentHitPoints = maxTP;

        meleeWeapon.simpleDamager.damageAmount = currentDamage;

        // ���⿡ �ʱ�ȭ
        soundManager = SoundManager.Instance;
        effectManager = EffectManager.Instance;
        impulseCam = ImpulseCam.Instance;
    }

    private void ChargeCP(Collider other)
    {
        {
            Debug.Log("cp�� ȸ���Ѵ�.");
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

        // �ǽð����� TP ����
        if (_damageable.CurrentHitPoints > 0f)
        {
            _damageable.CurrentHitPoints -= Time.deltaTime;
        }

        // �ǽð����� CP����
        if (IsDecreaseCP && CP > 0)
        {
            CP -= Time.deltaTime * decayTime;
            if (CP <= 0)
            {
                IsDecreaseCP = false;
                CP = 0;
                Debug.Log("���͵��� �ӵ��� ������� ���ƿ´�.");
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
        //  �����°� �ƴҶ��� ����
        if (PlayerFSM.GetState().ToString() == "PlayerDefenceState")
        {
            return;
        }
        if (PlayerFSM.GetState().ToString() == "PlayerParryState")
        {
            return;
        }
        if (PlayerFSM.GetState().ToString() == "PlayerDamagedState")
        {
            return;
        }

        PlayerFSM.Animator.SetTrigger("Damaged");
    }

    // �׾��� �� ȣ��Ǵ� �Լ�
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

    // Į ���带 ����� �� ����Ʈ�� �վ��
    // ��� �̷��� �ҰŶ�� �̸��� �ٲ�߰ڴ�
    public void SoundSword()
    {
        soundManager.PlaySFX("Attack_SE", transform);
        // ����Ʈ �̰� �����̼��� Į�� �����̼ǰ� �����.
        // Į�� ����Ʈ�� ������ �ٸ��Ƿ� �̰� ����Ʈ���� ���� �ѹ��� �ʿ���
        // ��ġ�� y ��ǥ�� Į�� ����, �������� �÷��̾� Ʈ����������
        GameObject slash = effectManager.SpawnEffect("SlashBlue2", transform.position);
        slash.transform.rotation = playerSword.transform.rotation;
        slash.transform.Rotate(90f, 180f, 0);
        float newY = playerSword.transform.position.y;
        slash.transform.position = new Vector3(slash.transform.position.x, newY, slash.transform.position.z);
        Destroy(slash, 0.7f);
    }

    public void SoundVoice()
    {
        soundManager.PlaySFX("Character_voice_SE", transform);
    }

    public void Shake()
    {
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
        if (_currentCheckpoint != null)
        {
            transform.position = _currentCheckpoint.transform.position;
            transform.rotation = _currentCheckpoint.transform.rotation;
        }
        else
        {
            Debug.LogError("üũ����Ʈ�� ���� ����");
        }

        // 1�� ���� ���̵� �ƿ�
        //yield return StartCoroutine(ScreenFader.FadeSceneIn());

        /// TODO - ���ذ�: �ʱ�ȭ �Լ��� ���� ���� ��

        // TP �ʱ�ȭ - ����ȵ�
        _damageable.ResetDamage();
        TP = maxTP;
        // CP �ʱ�ȭ
        currentCP = 0f;
    }
}
