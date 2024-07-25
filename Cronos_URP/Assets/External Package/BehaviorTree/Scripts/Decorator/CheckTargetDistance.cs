using UnityEngine;

class CheckTargetDistance : DecoratorNode
{
    public enum Comparison
    {
        Greater,
        Less
    }

    public float distance;
    public Comparison comparison;

    private GameObject _target;

    protected override void OnStart()
    {
        _target = blackboard.target;
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector3 toTarget = _target.transform.position - blackboard.monobehaviour.transform.position;
        bool checkDistance = toTarget.sqrMagnitude < distance * distance;

        if (comparison == Comparison.Greater && checkDistance == true)
        {
            return State.Failure;
        }
        else if (comparison == Comparison.Less && checkDistance == false)
        {
            return State.Failure;
        }

        var state = child.Update();

        return state;
    }
}
