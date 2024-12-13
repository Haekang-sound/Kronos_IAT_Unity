using UnityEngine;

class AnimatorSetUntrigger : ActionNode
{
    public string parameterName;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.gameObject == null)
        {
            return State.Failure;
        }

        var animator = context.animator;

        if (animator == null)
        {
            return State.Failure;
        }

        animator.ResetTrigger(parameterName);

        return State.Success;
    }
}