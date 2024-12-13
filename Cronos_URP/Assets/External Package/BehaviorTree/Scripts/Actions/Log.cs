using UnityEngine;

public class Log : ActionNode
{
    public string message;

    protected override void OnStart()
    {
        description = message;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Debug.Log($"{message}");
        return State.Success;
    }
}
