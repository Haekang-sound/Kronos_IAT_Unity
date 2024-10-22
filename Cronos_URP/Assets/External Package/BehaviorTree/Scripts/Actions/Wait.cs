using UnityEngine;

public class Wait : ActionNode
{
    public float duration = 1;
    float _elapse;

    protected override void OnStart()
    {
        _elapse = 0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float speed = 1;

        _elapse += blackboard.bulletTimeScalable.GetDeltaTime();

        if (_elapse > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
