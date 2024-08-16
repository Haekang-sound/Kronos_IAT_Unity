using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class MoveToTarget : ActionNode
{
    public float speed = 3.5f;
    public float stoppingDistance = 0.1f;
    public bool updateRotation = true;
    public bool useAnimationSpeed;
    public float acceleration = 100f;

    protected override void OnStart()
    {
        SetFollowNavmeshAgent(true);
        UseNavemeshAgentRotation(true);

        UpdateMoveToPosition();

        context.agent.stoppingDistance = stoppingDistance;
        if (useAnimationSpeed == true)
        {
            context.agent.speed = speed;
        }
        context.agent.destination = blackboard.moveToPosition;
        context.agent.updateRotation = updateRotation;
        context.agent.acceleration = acceleration;
    }

    protected override void OnStop()
    {
        SetFollowNavmeshAgent(false);
        UseNavemeshAgentRotation(false);
    }

    protected override State OnUpdate()
    {
        UpdateMoveToPosition();
        UpdateDestination();

        if (context.agent.pathPending)
        {
            return State.Running;
        }

        if (context.agent.remainingDistance < stoppingDistance)
        {
            return State.Success;
        }

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
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

    public void SetFollowNavmeshAgent(bool follow)
    {
        if (!follow && context.agent.enabled)
        {
            context.agent.ResetPath();
        }
        else if (follow && !context.agent.enabled)
        {
            context.agent.Warp(context.gameObject.transform.position);
        }

        //_followNavmeshAgent = follow;
        context.agent.enabled = follow;
    }

    public void UseNavemeshAgentRotation(bool use)
    {
        context.agent.updateRotation = use;
    }
}