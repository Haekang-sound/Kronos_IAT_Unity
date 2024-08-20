public class RepeatNTimes : DecoratorNode
{
    public int time;
    private int _remaintime;
    protected override void OnStart()
    {
        Reset();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var result = child.Update();

        if (result != State.Running)
        {
            _remaintime--;

            if (_remaintime <= 0)
            {
                Reset();
                return result;
            }
        }

        return State.Running;
    }

    private void Reset()
    {
        _remaintime = time;
    }
}
