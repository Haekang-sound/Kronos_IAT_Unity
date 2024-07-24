using UnityEngine;

class AnimatorSetTrigger : ActionNode
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
        if (blackboard.monobehaviour == null)
        {
            return State.Failure;
        }

        var animator = blackboard.monobehaviour.GetComponent<Animator>();

        if (animator == null)
        {
            return State.Failure;
        }

        animator.SetTrigger(parameterName);

        return State.Success;
    }
}