using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using static Controls;

/// <summary>
/// 이 코드는 Unity 게임 엔진을 사용하여 플레이어 입력을 관리하는 클래스를 정의합니다.
/// InputReader 클래스는 Unity의 새로운 InputSystem을 활용하여 마우스와 키보드 입력을 처리합니다.
/// 
/// Player의 인풋 액션의 대한 트리거를 정의 합니다.
/// </summary>
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
	static InputReader instance;
	public static InputReader GetInstance() { return instance; }

	public bool clickCondition = true;

	public Vector2 mouseDelta;
	public Vector2 moveComposite;

	public Action onMove;

	public Action onJumpStart;
	public Action onJumpPerformed;
	public Action onJumpCanceled;

	public Action onLAttackStart;
	public Action onLAttackPerformed;
	public Action onLAttackCanceled;

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

	public Action onInteratctiveStart;
	public Action onInteratctivePerformed;
	public Action onInteratctiveCanceled;

	public Action onFlashSlashStart;
	public Action onFlashSlashPerformed;
	public Action onFlashSlashCanceled;

	public bool IsLAttackPressed { get; set; } = false;
	public bool IsRAttackPressed { get; private set; } = false;

	private Controls controls;

	public void Awake()
	{
		instance = this;
	}

	private void OnEnable()
	{
		if (controls != null)
		{
			controls.Player.Enable();
			return;
		}
		controls = new Controls();
		controls.Player.SetCallbacks(this); // InputReader는 IPlayerActions를 상속받았다.
											// Actions을 세팅한다.
		controls.Player.Enable();           // 사용가능한 형태로 만든다.
	}

	public void OnDisable()
	{
		// 플레이어의 disable 함수를 호출한다.
		controls.Player.Disable();

	}

	// 카메라 이동을 담당하는! 
	public void OnLook(InputAction.CallbackContext context)
	{
		mouseDelta = context.ReadValue<Vector2>();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		moveComposite = context.ReadValue<Vector2>();
		onMove?.Invoke(); // 이동 발생여부를 검증한다.
	}

	public void OnJump(InputAction.CallbackContext context)
	{
		if (context.started) { onJumpStart?.Invoke(); }
		else if (context.performed) { onJumpPerformed?.Invoke(); }
		else if (context.canceled) { onJumpCanceled?.Invoke(); }
	}

	public void OnLAttack(InputAction.CallbackContext context)
	{
		if (context.started && Input.GetKeyDown(KeyCode.Mouse0))
		{
			IsLAttackPressed = true;
			onLAttackStart?.Invoke();
		}
		else if (context.performed && Input.GetKey(KeyCode.Mouse0))
		{
			onLAttackPerformed?.Invoke();
		}
		else if (context.canceled && Input.GetKeyUp(KeyCode.Mouse0))
		{
			IsLAttackPressed = false;
			onLAttackCanceled?.Invoke();
		}
	}

	// 우클릭
	public void OnRAttack(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			IsRAttackPressed = true;
			onRAttackStart?.Invoke();
		}
		else if (context.performed)
		{
			onRAttackPerformed?.Invoke();
		}
		else if (context.canceled)
		{
			IsRAttackPressed = false;
			onRAttackCanceled?.Invoke();
		}
	}

	// Q
	public void OnDeceleration_Q(InputAction.CallbackContext context)
	{
		if (context.started) onDecelerationStart?.Invoke();
		else if (context.performed) onDecelerationPerformed?.Invoke();
		else if (context.canceled) onDecelerationCanceled?.Invoke();
	}

	// 휠
	public void OnZoom(InputAction.CallbackContext context) { onZoom?.Invoke(); }

	// 휠클릭
	public void OnLockOn(InputAction.CallbackContext context)
	{
		if (context.started) onLockOnStart?.Invoke();
		else if (context.performed) onLockOnPerformed?.Invoke();
		else if (context.canceled) onLockOnCanceled?.Invoke();
	}

	// Tab
	public void OnUnlockAbility(InputAction.CallbackContext context)
	{
		if (context.started) onUnlockAbilityStart?.Invoke();
		else if (context.performed) onUnlockAbilityPerformed?.Invoke();
		else if (context.canceled) onUnlockAbilityCanceled?.Invoke();
	}

	// LShift
	public void OnRun_LShift(InputAction.CallbackContext context)
	{
		if (context.started) { onRunPerformed?.Invoke(); }
		else if (context.performed) { onRunStart?.Invoke(); }
		else if (context.canceled) { onRunCanceled?.Invoke(); }
	}

	// F
	public void OnInterActive_F(InputAction.CallbackContext context)
	{
		if (context.started) { onInteratctiveStart?.Invoke(); }
		else if (context.performed) { onInteratctivePerformed?.Invoke(); }
		else if (context.canceled) { onInteratctiveCanceled?.Invoke(); }
	}

	// E
	public void OnFlashSlash_E(InputAction.CallbackContext context)
	{
		if (context.started) { onFlashSlashStart?.Invoke(); }
		else if (context.performed) { onFlashSlashPerformed?.Invoke(); }
		else if (context.canceled) { onFlashSlashCanceled?.Invoke(); }
	}
}