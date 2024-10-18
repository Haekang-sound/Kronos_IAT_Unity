using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWait : ActionNode
{
    public float minDuration = 0;
    public float maxDuration = 1;

    [SerializeField] private float _duration = 1;
    [SerializeField] private float _elapse;

    protected override void OnStart()
    {
        _duration = Random.Range(minDuration, maxDuration + 1);
        _elapse = 0f;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        float speed = 1;

        _elapse += blackboard.bulletTimeScalable.GetDeltaTime();

        if (_elapse > _duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
