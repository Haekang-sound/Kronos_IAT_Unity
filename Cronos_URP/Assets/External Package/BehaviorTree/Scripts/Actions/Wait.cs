using UnityEngine;

public class Wait : ActionNode
{
    public float duration = 1;
    [SerializeField] float _elapse;

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

        if (BulletTime.Instance)
        {
            speed = BulletTime.Instance.GetCurrentSpeed();
        }

        _elapse += Time.deltaTime * speed;

        if (_elapse > duration)
        {
            return State.Success;
        }
        return State.Running;
    }
}
