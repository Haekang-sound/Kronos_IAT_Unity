using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// ��� stateMachine�� ����̵Ǵ� SateMachine Ŭ����
public abstract class StateMachine : MonoBehaviour
{
	private State currentState; // ����
	[Range(0.0f, 1.0f)] public float minFrame;
	public bool isPaused = false;

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
		if (!isPaused)
		{
			currentState?.Tick();
		}
	}

	private void FixedUpdate()
	{
		if (!isPaused)
		{
			currentState?.FixedTick();
		}
	}

	private void LateUpdate()
	{
		if (!isPaused)
		{
			currentState?.LateTick();
		}
	}

	public State GetState()
	{
		return currentState;
	}


}
