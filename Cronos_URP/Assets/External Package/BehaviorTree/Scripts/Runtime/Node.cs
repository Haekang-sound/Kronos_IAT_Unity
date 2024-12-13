using System;
using UnityEngine;

/// <summary>
/// 'Node' 클래스는 행동 트리에서 각 노드의 기본 클래스입니다.
/// 행동 트리의 각 단계를 처리하는 업데이트, 시작, 종료 메서드를 포함하며, 
/// 이를 통해 트리의 흐름을 제어합니다.
/// </summary>
public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success
    }

    [HideInInspector] public State state = State.Running;
    [HideInInspector] public bool started = false;
    [HideInInspector] public string guid;
    [HideInInspector] public Vector2 position;
    [HideInInspector] public Context context;
    [HideInInspector] public Blackboard blackboard;
    [TextArea] public string description;
    public bool drawGizmos = false;

    public State Update()
    {
        if (!started) 
        {
            OnStart();
            started = true;
        }

        state = OnUpdate();

        if (state == State.Failure || state == State.Success) 
        {
            OnStop();
            started = false;
        }

        return state;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    public virtual void OnDrawGizmos() { }

    protected abstract void OnStart();
    protected abstract void OnStop();
    protected abstract State OnUpdate();

    internal void Abort()
    {
        throw new NotImplementedException();
    }
}
