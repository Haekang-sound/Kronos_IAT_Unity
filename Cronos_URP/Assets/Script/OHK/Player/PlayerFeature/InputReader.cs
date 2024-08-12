using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

/// <summary>
/// �� �ڵ�� Unity ���� ������ ����Ͽ� �÷��̾� �Է��� �����ϴ� Ŭ������ �����մϴ�.
/// InputReader Ŭ������ Unity�� ���ο� InputSystem�� Ȱ���Ͽ� ���콺�� Ű���� �Է��� ó���մϴ�.
/// 
/// Player�� ��ǲ �׼��� ���� Ʈ���Ÿ� ���� �մϴ�.
/// </summary>
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
	public Vector2 mouseDelta; // ���콺 �̵������� �޾ƿ´�
	public Vector2 moveComposite;

	public Action onMove;

	public Action onJumpStart;      // ������ ���� �׼��� ��� ���� ����
	public Action onJumpPerformed;      // ������ ���� �׼��� ��� ���� ����
	public Action onJumpCanceled;      // ������ ���� �׼��� ��� ���� ����

	public Action onLAttackStart;   // ������ ���� �׼��� ��� ���� ����
	public Action onLAttackPerformed;   // ������ ���� �׼��� ��� ���� ����
	public Action onLAttackCanceled;   // ������ ���� �׼��� ��� ���� ����

	public Action onRAttackStart;
	public Action onRAttackPerformed;
	public Action onRAttackCanceled;

	public Action onDecelerationStart;
	public Action onDecelerationPerformed;
	public Action onDecelerationCanceled;

	public Action onZoom;

	public Action onLockOnStart;
	public Action onLockOnPerformed;
	public Action onLockOnCanceled;

	public Action onUnlockAbilityStart;
	public Action onUnlockAbilityPerformed;
	public Action onUnlockAbilityCanceled;

	public Action onRunStart;
	public Action onRunPerformed;
	public Action onRunCanceled;

	public bool IsLAttackPressed { get; set; } = false;
	public bool IsRAttackPressed { get; private set; } = false;

	private Controls controls;

	private void OnEnable()
	{
		if (controls != null)
		{
			controls.Player.Enable();
			return;
		}
		controls = new Controls();
		controls.Player.SetCallbacks(this); // InputReader�� IPlayerActions�� ��ӹ޾Ҵ�.
											// Actions�� �����Ѵ�.
		controls.Player.Enable();       // ��밡���� ���·� �����.
	}

	public void OnDisable()
	{
		// �÷��̾��� disable �Լ��� ȣ���Ѵ�.
		controls.Player.Disable();

	}

	// ī�޶� �̵��� ����ϴ�! 
	public void OnLook(InputAction.CallbackContext context) { mouseDelta = context.ReadValue<Vector2>(); }
	public void OnMove(InputAction.CallbackContext context)
	{
		moveComposite = context.ReadValue<Vector2>();
		onMove?.Invoke(); // �̵� �߻����θ� �����Ѵ�.
	}

	public void OnJumpDown(InputAction.CallbackContext context) { if (context.started) onJumpStart?.Invoke(); }
	public void OnJump(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}

		onJumpPerformed?.Invoke();// onJump�� null �� �ƴ϶�� �����Ѵ�.
	}
	public void OnJumpUp(InputAction.CallbackContext context) { if (context.canceled) onJumpCanceled?.Invoke(); } // onJump�� null �� �ƴ϶�� �����Ѵ�.
																							// ��Ŭ��
	public void OnLAttackDown(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Debug.Log("�۵�down");
			onLAttackStart?.Invoke();
			// L.Attack ���� ó��
			IsLAttackPressed = true;
		}
	}
	public void OnLAttack(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Debug.Log("�۵�");
			onLAttackPerformed?.Invoke();
			// L.Attack ó��
		}
	}
	public void OnLAttackUp(InputAction.CallbackContext context)
	{
		if (context.canceled)
		{
			Debug.Log("�۵�up");
			IsLAttackPressed = false;
			onLAttackCanceled?.Invoke();
			// L.Attack ���� ó��
		}
	}
	// ��Ŭ��
	public void OnRAttackDown(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			IsRAttackPressed = true;
			onRAttackStart?.Invoke();

		}
	}
		public void OnRAttack(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			onRAttackPerformed?.Invoke();
		}
	}
	public void OnRAttackUp(InputAction.CallbackContext context)
	{
		if (context.canceled)
		{
			IsRAttackPressed = false;
			onRAttackCanceled?.Invoke();
		}
	}

	// Q
	public void OnDecelerationDown(InputAction.CallbackContext context) { if (context.started) onDecelerationStart?.Invoke(); }
	public void OnDeceleration(InputAction.CallbackContext context) { if (context.performed) onDecelerationPerformed.Invoke(); }
	public void OnDecelerationUp(InputAction.CallbackContext context) { if (context.canceled) onDecelerationCanceled?.Invoke(); }

	// ��
	public void OnZoom(InputAction.CallbackContext context) { onZoom?.Invoke(); }


	public void OnLockOnDown(InputAction.CallbackContext context) { if (context.started) onLockOnStart?.Invoke(); }
	public void OnLockOn(InputAction.CallbackContext context) { if (context.performed) onLockOnPerformed?.Invoke(); Debug.Log("��������"); }
	public void OnLockOnUp(InputAction.CallbackContext context) { if (context.canceled) onLockOnCanceled?.Invoke(); }

	public void OnUnlockAbilityDown(InputAction.CallbackContext context)
	{
		if (context.started) onUnlockAbilityStart?.Invoke();
	}

	public void OnUnlockAbility(InputAction.CallbackContext context)
	{
		if (context.performed) onUnlockAbilityPerformed?.Invoke();

	}
	public void OnUnlockAbilityUp(InputAction.CallbackContext context)
	{
		if (context.canceled) onUnlockAbilityCanceled?.Invoke();
	}


	public void OnRunDown(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			onRunPerformed?.Invoke();
		}
	}
	public void OnRun(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			onRunStart?.Invoke();
		}
	}


	public void OnRunUp(InputAction.CallbackContext context)
	{
		if (context.canceled)
		{
			onRunCanceled?.Invoke();
		}
	}



}