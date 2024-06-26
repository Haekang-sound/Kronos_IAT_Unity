using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// ��� stateMachine�� ����̵Ǵ� SateMachine Ŭ����
public abstract class StateMachine : MonoBehaviour
{
	private State currentState; // ����
    
	// ���¸� �����ϴ� �Լ�
	public void SwitchState(State state)
	{
        currentState?.Exit();   // ���� ���¸� Ż���մϴ�.
        currentState = state;   // ���ο� ���·� �����մϴ�.
        currentState.Enter();   // ���ο� ���·� �����մϴ�.
	}

    private void Update()
    {
        // ���� ���¸� ������Ʈ�Ѵ�.
        currentState?.Tick();
    }

	private void FixedUpdate()
	{
		currentState?.FixedTick();
	}

	private void LateUpdate()
	{
		currentState?.LateTick();
	}

	public State GetState()
	{
		return currentState;
	}


}
