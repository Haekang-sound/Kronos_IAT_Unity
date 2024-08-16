using Message;
using UnityEngine;
using UnityEngine.Playables;

public class BossBehavior : MonoBehaviour, IMessageReceiver
{
    public bool drawGizmos;
    public GameObject target;
    public float targetDistance;

    public BehaviorTree phaseOne;
    public BehaviorTree phaseTwo;
    public BehaviorTree phaseTree;
    private Blackboard _blackboard;

    private Damageable _damageable;
    private PlayableDirector _playableDirector;
    private BehaviorTreeRunner _behaviortreeRunner;

    private bool _onPhaseOne;
    private bool _onPhaseTwo;
    private bool _onPhaseTree;


    void Awake()
    {
        _blackboard = new Blackboard();

        _damageable = GetComponent<Damageable>();
        _playableDirector = GetComponent<PlayableDirector>();
        _behaviortreeRunner = GetComponent<BehaviorTreeRunner>();
    }

    //void Start()
    //{
    //}

    void OnEnable()
    {
        _blackboard.target = target;
        _blackboard.monobehaviour = gameObject;
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
}
