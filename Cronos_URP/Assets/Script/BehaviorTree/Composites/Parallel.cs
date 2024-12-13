using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 자식 노드들을 병렬로 실행하며, 자식 중 하나라도 실패하면 전체가 실패하도록 하는 노드입니다.
/// 모든 자식 노드의 실행 상태를 추적하고, 실행 중인 노드가 있으면 계속 실행합니다.
/// </summary>
public class Parallel : CompositeNode
{
    List<State> childrenLeftToExecute = new List<State>();

    protected override void OnStart()
    {
        childrenLeftToExecute.Clear();
        children.ForEach(a => {
            childrenLeftToExecute.Add(State.Running);
        });
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        bool stillRunning = false;
        for (int i = 0; i < childrenLeftToExecute.Count(); ++i)
        {
            if (childrenLeftToExecute[i] == State.Running)
            {
                var status = children[i].Update();
                if (status == State.Failure)
                {
                    AbortRunningChildren();
                    return State.Failure;
                }

                if (status == State.Running)
                {
                    stillRunning = true;
                }

                childrenLeftToExecute[i] = status;
            }
        }

        return stillRunning ? State.Running : State.Success;
    }

    void AbortRunningChildren()
    {
        for (int i = 0; i < childrenLeftToExecute.Count(); ++i)
        {
            if (childrenLeftToExecute[i] == State.Running)
            {
                children[i].Abort();
            }
        }
    }
}