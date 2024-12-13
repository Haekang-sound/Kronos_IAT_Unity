
/// <summary>
/// 자식 노드를 지정된 횟수만큼 반복 실행하는 노드입니다.
/// 자식 노드가 `Running` 상태일 때는 반복을 계속하고, 그 외의 상태일 때는 남은 반복 횟수를 추적합니다.
/// 반복 횟수를 다 채우면 최종 결과를 반환합니다.
/// </summary>
public class RepeatNTimes : DecoratorNode
{
    public int time;
    private int _remaintime;
    protected override void OnStart()
    {
        Reset();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var result = child.Update();

        if (result != State.Running)
        {
            _remaintime--;

            if (_remaintime <= 0)
            {
                Reset();
                return result;
            }
        }

        return State.Running;
    }

    private void Reset()
    {
        _remaintime = time;
    }
}
