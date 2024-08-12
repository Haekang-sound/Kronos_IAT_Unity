using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;

/// <summary>
/// 이 코드는 Unity 게임 엔진을 사용하여 플레이어 입력을 관리하는 클래스를 정의합니다.
/// InputReader 클래스는 Unity의 새로운 InputSystem을 활용하여 마우스와 키보드 입력을 처리합니다.
/// 
/// Player의 인풋 액션의 대한 트리거를 정의 합니다.
/// </summary>
public class InputReader : MonoBehaviour, Controls.IPlayerActions
{
	public Vector2 mouseDelta; // 마우스 이동정보를 받아온다
	public Vector2 moveComposite;

	public Action onMove;

	public Action onJumpStart;      // 점프의 대한 액션을 담기 위한 변수
	public Action onJumpPerformed;      // 점프의 대한 액션을 담기 위한 변수
	public Action onJumpCanceled;      // 점프의 대한 액션을 담기 위한 변수

	public Action onLAttackStart;   // 공격의 대한 액션을 담기 위한 변수
	public Action onLAttackPerformed;   // 공격의 대한 액션을 담기 위한 변수
	public Action onLAttackCanceled;   // 공격의 대한 액션을 담기 위한 변수

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
		controls.Player.SetCallbacks(this); // InputReader는 IPlayerActions를 상속받았다.
											// Actions을 세팅한다.
		controls.Player.Enable();       // 사용가능한 형태로 만든다.
	}

	public void OnDisable()
	{
		// 플레이어의 disable 함수를 호출한다.
		controls.Player.Disable();

	}

	// 카메라 이동을 담당하는! 
	public void OnLook(InputAction.CallbackContext context) { mouseDelta = context.ReadValue<Vector2>(); }
	public void OnMove(InputAction.CallbackContext context)
	{
		moveComposite = context.ReadValue<Vector2>();
		onMove?.Invoke(); // 이동 발생여부를 검증한다.
	}

	public void OnJumpDown(InputAction.CallbackContext context) { if (context.started) onJumpStart?.Invoke(); }
	public void OnJump(InputAction.CallbackContext context)
	{
		if (!context.performed)
		{
			return;
		}

		onJumpPerformed?.Invoke();// onJump가 null 이 아니라면 실행한다.
	}
	public void OnJumpUp(InputAction.CallbackContext context) { if (context.canceled) onJumpCanceled?.Invoke(); } // onJump가 null 이 아니라면 실행한다.
																							// 좌클릭
	public void OnLAttackDown(InputAction.CallbackContext context)
	{
		if (context.started)
		{
			Debug.Log("작동down");
			onLAttackStart?.Invoke();
			// L.Attack 시작 처리
			IsLAttackPressed = true;
		}
	}
	public void OnLAttack(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			Debug.Log("작동");
			onLAttackPerformed?.Invoke();
			// L.Attack 처리
		}
	}
	public void OnLAttackUp(InputAction.CallbackContext context)
	{
		if (context.canceled)
		{
			Debug.Log("작동up");
			IsLAttackPressed = false;
			onLAttackCanceled?.Invoke();
			// L.Attack 종료 처리
		}
	}
	// 우클릭
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

	// 휠
	public void OnZoom(InputAction.CallbackContext context) { onZoom?.Invoke(); }


	public void OnLockOnDown(InputAction.CallbackContext context) { if (context.started) onLockOnStart?.Invoke(); }
	public void OnLockOn(InputAction.CallbackContext context) { if (context.performed) onLockOnPerformed?.Invoke(); Debug.Log("누르는중"); }
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