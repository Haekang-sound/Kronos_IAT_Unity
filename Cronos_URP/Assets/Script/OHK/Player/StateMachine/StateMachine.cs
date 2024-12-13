using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ��� stateMachine�� ����̵Ǵ� SateMachine Ŭ����
/// 
/// ohk    v1
/// </summary>
public abstract class StateMachine : MonoBehaviour
{
	private State _currentState;
	public bool isPaused = false;
	[Range(0.0f, 1.0f)] public float minFrame;

	private void FixedUpdate()
	{
		if (!isPaused)
		{
			_currentState?.FixedTick();
		}
	}

	private void Update()
	{
		if (!isPaused)
		{
			_currentState?.Tick();
		}
	}

	private void LateUpdate()
	{
		if (!isPaused)
		{
			_currentState?.LateTick();
		}
	}

	public State GetState()
	{
		return _currentState;
	}

	public void SwitchState(State state)
	{
		_currentState?.Exit();
		_currentState = state;
		_currentState.Enter();
	}
}
