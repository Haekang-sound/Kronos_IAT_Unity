using System;
using UnityEditor.Build;
using UnityEditor.ShaderGraph;
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

    public Action onSwitchingStart;
    public Action onSwitchingPerformed;
    public Action onSwitchingCanceled;

    public Action onZoom;

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

    public void OnJumpDown(InputAction.CallbackContext context) { onJumpStart?.Invoke(); }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        onJumpPerformed?.Invoke();// onJump�� null �� �ƴ϶�� �����Ѵ�.
    }
    public void OnJumpUp(InputAction.CallbackContext context) { onJumpCanceled?.Invoke(); } // onJump�� null �� �ƴ϶�� �����Ѵ�.
                                                                                            // ��Ŭ��
    public void OnLAttackDown(InputAction.CallbackContext context) { onLAttackStart?.Invoke(); }
    public void OnLAttack(InputAction.CallbackContext context)
    {
        IsLAttackPressed = true;
        onLAttackPerformed?.Invoke();
    }
    public void OnLAttackUp(InputAction.CallbackContext context)
    {
        IsLAttackPressed = false;
        onLAttackCanceled?.Invoke();
    }
    // ��Ŭ��
    public void OnRAttackDown(InputAction.CallbackContext context) { onRAttackStart?.Invoke(); }
    public void OnRAttack(InputAction.CallbackContext context)
    {
        IsRAttackPressed = true;
        onRAttackPerformed?.Invoke();
    }
    public void OnRAttackUp(InputAction.CallbackContext context)
    {
        IsRAttackPressed = false;
        onRAttackCanceled?.Invoke();
    }
    // Q
    public void OnSwitchingDown(InputAction.CallbackContext context) { onSwitchingStart?.Invoke(); }
    public void OnSwitching(InputAction.CallbackContext context) { onSwitchingPerformed?.Invoke(); }
    public void OnSwitchingUp(InputAction.CallbackContext context) { onSwitchingCanceled?.Invoke(); }

    // ��
    public void OnZoom(InputAction.CallbackContext context) { onZoom?.Invoke(); }
}