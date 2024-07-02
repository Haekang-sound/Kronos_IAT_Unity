using System.Globalization;
using System.Resources;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public abstract class PlayerBaseState : State
{
	// ������ �б��������� ����
	protected readonly PlayerStateMachine stateMachine;
	Vector3 moveDirection;

	protected PlayerBaseState(PlayerStateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}

	/// <summary>
	/// ī�޶� ������ �������
	/// Player�� �̵������� ���Ѵ�.
	/// </summary>
	protected void CalculateMoveDirection()
	{
		// ������Ʈ �ӽſ� ����ִ� ī�޶� ������ �������
		// ī�޶��� ����, �¿� ���͸� �����Ѵ�.
		Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
		Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

		// �̵����ͻ���,
		// ī�޶��� ���溤�Ϳ� ��ǲ�� move.y ��ġ�� ���Ѵ�,
		// ī�޶��� �¿캤�Ϳ� ��ǲ�� movecomposite.x�� ���Ѵ�.
		moveDirection	= cameraForward.normalized * stateMachine.InputReader.moveComposite.y	// ����
								+ cameraRight.normalized * stateMachine.InputReader.moveComposite.x;    // �Ĺ�

		// ���¸ӽ��� �ӵ��� �̵����Ϳ� �ӷ��� ���̴�.
		stateMachine.Velocity.x = moveDirection.x * stateMachine.Player.moveSpeed;
		stateMachine.Velocity.y = 0f;
		stateMachine.Velocity.z = moveDirection.z * stateMachine.Player.moveSpeed;
	}

	/// <summary>
	/// �÷��̾ �̵��������� ȸ����Ų��.
	/// </summary>
	protected void FaceMoveDirection()
	{

		Vector3 faceDirection = new(stateMachine.Velocity.x, 0f, stateMachine.Velocity.z);

		// �̵��ӵ��� ���ٸ�
		if (faceDirection == Vector3.zero)
		{
			// �ƹ��͵� ���� �ʰڴ�.
			return;
		}
		// �÷��̾��� ȸ���� ���� ���������� ���·� �̷������. 
		stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.Player.lookRotationDampFactor * Time.fixedDeltaTime));
	}

	/// <summary>
	/// Player�� ���� �ִ� �����͸� �����ؼ�
	/// Player�� �̵���Ų��
	/// </summary>
	protected void Move()
	{
		//Debug.Log($"speed : {stateMachine.Animator.speed}, Player speed {stateMachine.Player.moveSpeed}");
		stateMachine.Rigidbody.AddForce(stateMachine.Velocity * stateMachine.Animator.speed * stateMachine.Player.moveSpeed * Time.fixedDeltaTime - GetPlayerHorizentalVelocity(), ForceMode.VelocityChange);
	}

	protected Vector3 GetPlayerHorizentalVelocity()
	{
		Vector3 playerHorizentalvelocity = stateMachine.Rigidbody.velocity;
		playerHorizentalvelocity.y = 0f;
		return playerHorizentalvelocity;
	}


	protected void MoveOnAnimation()
	{
		float animationSpeed = stateMachine.Animator.deltaPosition.magnitude;

		// test
		Vector3 direction = stateMachine.transform.forward.normalized;

		// ���ο� ���� ���
		Vector3 newDeltaPosition = direction * animationSpeed;

		stateMachine.transform.Translate(newDeltaPosition);
	}


	public bool IsGrounded()
	{
		RaycastHit hit;
		float distance = 0.1f;
		bool isGrounded = Physics.Raycast(stateMachine.transform.position, Vector3.down, out hit, distance);
		//Debug.Log($"���� ��ҳ�? {isGrounded}");
		// Raycast�� ����Ͽ� ���� üũ
		return isGrounded;
	}

}