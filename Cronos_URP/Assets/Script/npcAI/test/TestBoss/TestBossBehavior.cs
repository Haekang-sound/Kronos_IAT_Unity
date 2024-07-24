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

    private BehaviorTreeRunner _btRunner;
    private PlayableDirector _playableDirector;

    void Awake()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        _btRunner = GetComponent<BehaviorTreeRunner>();
    }

    void OnEnable()
    {
        _btRunner.tree.blackboard.monobehaviour = gameObject;
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
            _btRunner.tree.blackboard.target = target;
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

    private void Damaged()
    {
    }

    private void Dead()
    {
    }
}
