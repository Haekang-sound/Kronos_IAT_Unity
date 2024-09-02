public class ResetTimer : ActionNode
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
        blackboard.timer = resetTime;
        return State.Success;

    }
}
