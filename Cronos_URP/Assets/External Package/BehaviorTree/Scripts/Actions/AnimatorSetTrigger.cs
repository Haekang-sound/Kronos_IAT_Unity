using UnityEngine;

class AnimatorSetTrigger : ActionNode
{
    public string parameterName;
    public float waitTime = 0.5f;

    //[SerializeField]
    private float _timer;

    protected override void OnStart()
    {
        var animator = context.animator.GetComponent<Animator>();
        animator.SetTrigger(parameterName);
        _timer = waitTime;
    }

    protected override void OnStop()
    {
        _timer = 0f;
    }

    protected override State OnUpdate()
    {
        if (context.gameObject == null)
        {
            return State.Failure;
        }

        _timer -= Time.deltaTime * BulletTime.Instance.GetCurrentSpeed();

        if(_timer < 0f)
        {
            return State.Success;
        }

        return State.Running;
    }
}