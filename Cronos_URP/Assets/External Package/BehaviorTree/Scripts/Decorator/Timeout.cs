using UnityEditor;
using UnityEngine;

public class Timeout : DecoratorNode
{
    public float duration = 1.0f;
    float _startTime;

    protected override void OnStart()
    {
        _startTime = Time.time;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Time.time - _startTime > duration)
        {
            return State.Failure;
        }

        return child.Update();
    }
}