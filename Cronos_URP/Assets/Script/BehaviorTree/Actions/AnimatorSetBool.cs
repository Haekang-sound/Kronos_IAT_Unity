
/// <summary>
/// 지정된 애니메이터의 특정 부울 파라미터 값을 설정하고, 
/// 설정 완료 후 성공 상태를 반환합니다.
/// </summary>
class AnimatorSetBool : ActionNode
{
    public string parameterName;
    public bool value;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var animator = context.animator;

        if(animator == null)
        {
            return State.Failure;
        }

        animator.SetBool(parameterName, value);

        return State.Success;
    }
}
