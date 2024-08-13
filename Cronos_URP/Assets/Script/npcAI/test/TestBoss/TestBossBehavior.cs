using Message;
using UnityEngine;
using UnityEngine.Playables;

[DefaultExecutionOrder(100)]
[RequireComponent(typeof(EnemyController))]
public class TestBossBehavior : MonoBehaviour, IMessageReceiver
{
    public bool drawGizmos;
    public GameObject target;
    public float targetDistance;

    public BehaviorTree phaseOne;
    public BehaviorTree phaseTwo;
    public BehaviorTree phaseTree;

    private bool _phaseTwo;
    private bool _phaseTree;
    private Blackboard _blackboard;

    private Damageable _damageable;
    private BehaviorTreeRunner _btRunner;
    private PlayableDirector _playableDirector;

    void Awake()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        _damageable = GetComponent<Damageable>();
        _btRunner = GetComponent<BehaviorTreeRunner>();

        _blackboard = new Blackboard();
    }

    void OnEnable()
    {
        //_btRunner.tree.blackboard.monobehaviour = gameObject;
        _blackboard.target = target;
        _blackboard.monobehaviour = gameObject;

        _btRunner.tree = phaseOne;
    }

    void Start()
    {
    }    

    void OnDisable()
    {
        _btRunner.tree.blackboard.target = null;
        _btRunner.tree.blackboard.monobehaviour = null;
    }

    void Update()
    {
        Vector3 toTarget = target.transform.position - transform.position;
        bool checkDistance = toTarget.sqrMagnitude < targetDistance * targetDistance;

        if (target && checkDistance)
        {
            _btRunner.tree.blackboard = _blackboard;
            //_btRunner.tree.blackboard.target = target;
            _btRunner.Bind();
        }


        if(_phaseTwo == false && _damageable.GetHealthPercentage() < 70f)
        {
            ChangePhase(phaseTwo);
            _phaseTwo = true;
        }
        
        if(_phaseTree == false && _damageable.GetHealthPercentage() < 30f)
        {
            ChangePhase(phaseTree);
            _phaseTree = true;
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false) return;

        // 공격 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetDistance);
    }

    public void OnReceiveMessage(MessageType type, object sender, object data)
    {
        switch (type)
        {
            case MessageType.DAMAGED:
                Damaged();
                break;
            case MessageType.DEAD:
                Dead();
                break;
            case MessageType.RESPAWN:
                break;
            default:
                return;

        }
    }
    public void ChangePhase(BehaviorTree bt)
    {
        _btRunner.tree = bt;
        _btRunner.tree.blackboard = _blackboard;
        _btRunner.Bind();
    }

    private void Damaged()
    {
    }

    private void Dead()
    {
    }
}
