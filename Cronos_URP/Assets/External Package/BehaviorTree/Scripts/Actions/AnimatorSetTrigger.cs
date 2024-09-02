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
        if (context.gameObject == null)
        {
            return State.Failure;
        }

        var animator = context.animator.GetComponent<Animator>();

        if (animator == null)
        {
            return State.Failure;
        }

        animator.SetTrigger(parameterName);

        return State.Success;
    }
}