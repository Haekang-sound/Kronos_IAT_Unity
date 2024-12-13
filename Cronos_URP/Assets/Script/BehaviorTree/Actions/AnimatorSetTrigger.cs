using UnityEngine;

/// <summary>
/// 지정된 트리거를 활성화하고, 
/// 주어진 시간 동안 대기한 후 성공 상태를 반환합니다.
/// </summary>
class AnimatorSetTrigger : ActionNode
{
    public string parameterName;
    public float waitTime = 0.5f;

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


        _timer -= blackboard.bulletTimeScalable.GetDeltaTime();

        if(_timer < 0f)
        {
            return State.Success;
        }

        return State.Running;
    }
}