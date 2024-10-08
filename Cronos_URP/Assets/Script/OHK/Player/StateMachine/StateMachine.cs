using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// 모든 stateMachine의 기반이되는 SateMachine 클래스
public abstract class StateMachine : MonoBehaviour
{
	private State currentState; // 상태
	[Range(0.0f, 1.0f)] public float minFrame;
	public bool isPaused = false;

	// 상태를 변경하는 함수
	public void SwitchState(State state)
	{
		currentState?.Exit();   // 현재 상태를 탈출합니다.
		currentState = state;   // 새로운 상태로 변경합니다.
		currentState.Enter();   // 새로운 상태로 돌입합니다.
	}

	private void Update()
	{
		// 현재 상태를 업데이트한다.
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
