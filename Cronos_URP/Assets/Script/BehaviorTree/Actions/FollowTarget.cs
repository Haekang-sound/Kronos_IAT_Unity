﻿using UnityEngine;

/// <summary>
/// 적 캐릭터가 타겟을 추적하며 이동할 때 사용됩니다.
/// 이동 속도, 회전 업데이트, 목표 위치 등을 설정하고, 
/// 타겟에 도달하면 성공 상태를 반환합니다.
/// </summary>
public class FollowTarget : ActionNode
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
    }

    protected override void OnStop()
    {
        enemyController.SetFollowNavmeshAgent(false);
        enemyController.UseNavemeshAgentRotation(false);
    }

    protected override State OnUpdate()
    {

        UpdateMoveToPosition();
        UpdateDestination();

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

        if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
        {
            return State.Failure;
        }

        return State.Running;
    }

    public override void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(context.transform.position, stoppingDistance);
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