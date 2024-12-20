﻿using UnityEngine;

class CheckTargetDistance : ActionNode
{
    public enum Comparison
    {
        Greater,
        Less
    }

    public float distance;
    public Comparison comparison;

    //private GameObject _target;

    protected override void OnStart()
    {
        //_target = blackboard.target;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 toTarget = blackboard.target.transform.position - context.transform.position;
        bool checkDistance = toTarget.sqrMagnitude < distance * distance;

        if (comparison == Comparison.Greater && checkDistance == true)
        {
            return State.Failure;
        }
        else if (comparison == Comparison.Less && checkDistance == false)
        {
            return State.Failure;
        }

        return State.Success;
    }
    public override void OnDrawGizmos()
    {
        if(context == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(context.transform.position, distance);
    }
}
