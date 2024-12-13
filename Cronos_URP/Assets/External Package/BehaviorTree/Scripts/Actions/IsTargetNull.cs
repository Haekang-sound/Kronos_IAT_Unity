using UnityEngine;

class IsTargetNull : ActionNode
{
    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.target == null)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}