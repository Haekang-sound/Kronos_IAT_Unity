using UnityEngine;

/// <summary>
/// 이동할 목표를 설정하고, 이동을 시작한 후 목표에 도달하면 성공 상태를 반환합니다.
/// 이동 중에 목표가 도달 불가능하거나 경로가 부분적일 경우 실패 상태를 반환합니다.
/// </summary>
public class MoveToTarget : ActionNode
{
    public float speed = 3.5f;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public bool useAnimationSpeed;
    public float acceleration = 100f;

    private EnemyController enemyController;

    protected override void OnStart()
    {
        enemyController = context.gameObject.GetComponent<EnemyController>();

        enemyController.SetFollowNavmeshAgent(true);
        enemyController.UseNavemeshAgentRotation(true);

        context.agent.stoppingDistance = stoppingDistance;
        if (useAnimationSpeed == true)
        {
            context.agent.speed = speed;
        }
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;

        UpdateMoveToPosition();
        UpdateDestination();
    }

    protected override void OnStop()
    {
        enemyController.SetFollowNavmeshAgent(false);
        enemyController.UseNavemeshAgentRotation(false);
    }

    protected override State OnUpdate()
    {


        if (!useAnimationSpeed)
        {
            context.agent.acceleration = acceleration;
            context.agent.speed = speed;
        }

        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < stoppingDistance)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial ||
            context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }

    private void UpdateDestination()
    {
        context.agent.SetDestination(blackboard.moveToPosition);
    }

    private void UpdateMoveToPosition()
    {
        if (blackboard.target == null)
        {
            Debug.Log("타깃을 찾을 수 없음");
        }
        else
        {
            blackboard.moveToPosition = blackboard.target.transform.position;
        }
    }
}