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
        if(blackboard.monobehaviour == null)
        {
            return State.Failure;
        }

        var animator = blackboard.monobehaviour.GetComponent<Animator>();

        if(animator == null)
        {
            return State.Failure;
        }

        animator.SetBool(parameterName, value);

        return State.Success;
    }
}
