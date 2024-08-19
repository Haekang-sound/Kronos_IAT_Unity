public class CheckTimer : ActionNode
{
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
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
       
    }
}
