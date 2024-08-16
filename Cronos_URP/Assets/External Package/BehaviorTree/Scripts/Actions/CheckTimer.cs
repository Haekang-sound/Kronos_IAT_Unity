using UnityEngine;

public class CheckTimer : ActionNode
{
    public float resetTime;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (blackboard.timer < 0f)
        {
            blackboard.timer = resetTime;
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
       
    }
}
