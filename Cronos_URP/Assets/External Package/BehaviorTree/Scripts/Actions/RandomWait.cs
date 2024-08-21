using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWait : ActionNode
{
    public float minDuration = 0;
    public float maxDuration = 1;

    private float _duration = 1;
    private float _startTime;

    protected override void OnStart()
    {
        _startTime = Time.time;
        _duration = Random.Range(minDuration, maxDuration + 1);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (Time.time - _startTime > _duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
