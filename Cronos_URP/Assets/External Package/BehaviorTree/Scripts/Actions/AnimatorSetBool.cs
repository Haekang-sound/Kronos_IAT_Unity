using UnityEngine;
class AnimatorSetBool : ActionNode
{
    public string parameterName;
    public bool value;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var animator = context.animator;

        if(animator == null)
        {
            return State.Failure;
        }

        animator.SetBool(parameterName, value);

        return State.Success;
    }
}
