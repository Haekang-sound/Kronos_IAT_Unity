using Message;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;

public class BossBehavior : MonoBehaviour, IMessageReceiver
{
    public bool drawGizmos;
    public GameObject target;

    public float targetDistance;
    public float rotationSpeed = 1.0f;

    public BehaviorTree phaseOne;
    public BehaviorTree phaseTwo;
    public BehaviorTree phaseTree;
    private Blackboard _blackboard;

    [SerializeField]
    public EnemyController controller;

    private Animator _animator;

    private Damageable _damageable;
    private MeleeWeapon _meleeWeapon;
    private PlayableDirector _playableDirector;
    private BehaviorTreeRunner _behaviortreeRunner;

    private bool _onPhaseOne;
    private bool _onPhaseTwo;
    private bool _onPhaseTree;


    void Awake()
    {
        _blackboard = new Blackboard();

        controller = GetComponent<EnemyController>();

        _animator = GetComponent<Animator>();
        _damageable = GetComponent<Damageable>();
        _meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        _playableDirector = GetComponent<PlayableDirector>();
        _behaviortreeRunner = GetComponent<BehaviorTreeRunner>();
    }

    //void Start()
    //{
    //}

    void OnEnable()
    {
        SceneLinkedSMB<BossBehavior>.Initialise(_animator, this);

        _blackboard.target = target;
        //_blackboard.monobehaviour = gameObject;
    }

    //private void OnDisable()
    //{
    //}

    //private void OnDisable()
    //{
    //}

    //void OnDestroy()
    //{
    //}

    void Update()
    {
        UpdateBehaviorTree();
    }

    //void FixedUpdate()
    //{
    //}

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        // 감지 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetDistance);
    }

    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        var dmgMsg = (Damageable.DamageMessage)data;

        switch (type)
        {
            case MessageType.DAMAGED:
                EffectManager.Instance.CreateHitFX(dmgMsg, transform);
                break;
            case MessageType.DEAD:
                EffectManager.Instance.CreateHitFX(dmgMsg, transform);
                break;
            case MessageType.RESPAWN:
                break;
            default:
                return;

        }
    }

    //////////////////////////////////////////////////////////////////////////////////////

    private void UpdateBehaviorTree()
    {
        if (_onPhaseOne == false)
        {
            Vector3 toTarget = target.transform.position - transform.position;
            bool checkDistance = toTarget.sqrMagnitude < targetDistance * targetDistance;

            if (checkDistance)
            {
                ChangePhase(phaseOne);
                _onPhaseOne = true;
            }
        }

        if (_onPhaseTwo == false && _damageable.GetHealthPercentage() < 70f)
        {
            ChangePhase(phaseTwo);
            _onPhaseTwo = true;
        }

        if (_onPhaseTree == false && _damageable.GetHealthPercentage() < 30f)
        {
            ChangePhase(phaseTree);
            _onPhaseTree = true;
        }
    }

    public void ChangePhase(BehaviorTree bt)
    {
        _behaviortreeRunner.tree = bt;
        _behaviortreeRunner.tree.blackboard = _blackboard;
        _behaviortreeRunner.Bind();
    }

    public void LookAtTarget()
    {
        if (target == null) return;

        // 바라보는 방향 설정
        var lookPosition = target.transform.position - transform.position;
        lookPosition.y = 0;
        var rotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
    }

    public void Strafe(bool isRingth = true)
    {
        if (target == null) return;

        Vector3 offsetPlayer = target.transform.position - transform.position;

        if(isRingth == false)
        {
            offsetPlayer = transform.position - target.transform.position;
        }

        Vector3 direction = Vector3.Cross(offsetPlayer, Vector3.up);
        controller.SetTarget(transform.position + direction);

        LookAtTarget();
    }
    public void BeginAttack()
    {
        _meleeWeapon.BeginAttack();
    }

    public void EndAttack()
    {
        _meleeWeapon.EndAttack();
    }

    public void BeginAiming()
    {
        rotationSpeed = 5f;
    }

    public void StopAiming()
    {
        rotationSpeed = 0f;
    }

    public void ResetAiming()
    {
        rotationSpeed = 1f;
    }
}
