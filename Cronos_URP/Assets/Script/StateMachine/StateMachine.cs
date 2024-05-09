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
        // ���콺�� UI�� Ŭ�� ���̶�� ���� ������ �����ϴ�... by mic
        // �̰� UI �� Ŭ�� > UI�� ���콺�� �ű�� ���������...
        //if (EventSystem.current.IsPointerOverGameObject())
        //    return;

        currentState?.Exit();   // ���� ���¸� Ż���մϴ�.
        currentState = state;   // ���ο� ���·� �����մϴ�.
        currentState.Enter();   // ���ο� ���·� �����մϴ�.
	}

    // Update is called once per frame
    private void Update()
    {
        // ���� ���¸� �����Ѵ�.
        currentState?.Tick();
    }

	public State GetState()	// ������¸� ��ȯ���ش�
	{
		return currentState;
	}


}
