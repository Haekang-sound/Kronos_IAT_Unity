using System;
using System.Globalization;
using System.Resources;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// �÷��̾��� �⺻ ���¸� ����
/// ��� ������ �⺻�� �Ǹ� ���⼭���� �Ļ���..
/// 
/// ohk    v1 v1
/// </summary>
public abstract class PlayerBaseState : State
{
	// ������ �б��������� ����
	protected readonly PlayerStateMachine _stateMachine;
	private Vector3 _moveDirection;
	private SlopeData _slopeData;
	private const float _rayDistance = 2f;
	private RaycastHit _sloperHit;
	private int _groundLayer = 1 << LayerMask.NameToLayer("Ground");
	private float _maxSlopeAngle = 30f;

	protected PlayerBaseState(PlayerStateMachine stateMachine)
	{
		this._stateMachine = stateMachine;
		_slopeData = stateMachine.Player.capsuleColldierUtility.SlopeData;
	}
	
	// ���ν�Ƽ�� �߸��Ǹ� ����
	protected bool VelocityCheck()
	{
		if (!float.IsNaN(_stateMachine.Animator.deltaPosition.x )
		&& !float.IsNaN(_stateMachine.Animator.deltaPosition.y)
		&& !float.IsNaN((_stateMachine.Animator.deltaPosition.z)))
		{
			return false;	
		}
		return true;
	}

	/// <summary>
	/// ī�޶� ������ �������
	/// Player�� �̵������� ���Ѵ�.
	/// </summary>
	protected void CalculateMoveDirection()
	{
		if (_stateMachine.InputReader == null)
			return;

		// ������Ʈ �ӽſ� ����ִ� ī�޶� ������ �������
		// ī�޶��� ����, �¿� ���͸� �����Ѵ�.
		Vector3 cameraForward = new(_stateMachine.MainCamera.forward.x, 0, _stateMachine.MainCamera.forward.z);
		Vector3 cameraRight = new(_stateMachine.MainCamera.right.x, 0, _stateMachine.MainCamera.right.z);

		// �̵����ͻ����� �����Ѵ�.
		_moveDirection = cameraForward.normalized * _stateMachine.InputReader.moveComposite.y 
								+ cameraRight.normalized * _stateMachine.InputReader.moveComposite.x;    

		// ���¸ӽ��� �ӵ��� �̵����Ϳ� �ӷ��� ���̴�.
		_stateMachine.velocity.x = _moveDirection.x * _stateMachine.Player.moveSpeed;
		_stateMachine.velocity.y = 0f;
		_stateMachine.velocity.z = _moveDirection.z * _stateMachine.Player.moveSpeed;
	}

	/// <summary>
	/// ��ǲŰ�� �̿��� ������ ��������
	/// �÷��̾ ȸ����Ų��.
	/// </summary>
	protected void FaceMoveDirection()
	{
		Vector3 faceDirection = new(_stateMachine.velocity.x, 0f, _stateMachine.velocity.z);

		// �̵��ӵ��� ���ٸ�
		// �������� �ʴ´�.
		if (faceDirection == Vector3.zero)
		{
			return;
		}

		// �÷��̾��� ȸ���� ���� ���������� ���·� �̷������. 
		_stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(_stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), _stateMachine.Player.lookRotationDampFactor * Time.fixedDeltaTime));

	}

	/// <summary>
	/// ���� ������ ��������
	/// �÷��̾ ȸ����Ų��.
	/// </summary>
	/// <param name="faceDir"></param>
	protected void FaceMoveDirection(Vector3 faceDir)
	{
		faceDir.y = 0f;

		// �̵��ӵ��� ���ٸ�
		// �������� �ʴ´�.
		if (faceDir == Vector3.zero)
		{
			return;
		}
		// �÷��̾��� ȸ���� ���� ���������� ���·� �̷������. 
		_stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(_stateMachine.transform.rotation, Quaternion.LookRotation(faceDir), _stateMachine.Player.lookRotationDampFactor * Time.fixedDeltaTime));
	}

	/// <summary>
	/// Player�� ���� �ִ� �����͸� �����ؼ�
	/// Player�� �̵���Ų��
	/// </summary>
	protected void Move()
	{
		Vector3 velocity = _stateMachine.velocity;
		Vector3 gravity = Vector3.down * Mathf.Abs(_stateMachine.Rigidbody.velocity.y);

		_stateMachine.Rigidbody.velocity = velocity * Time.fixedDeltaTime * _stateMachine.Player.moveSpeed
			 * _stateMachine.Animator.speed + gravity;
		Float();
	}

	protected Vector3 GetPlayerHorizentalVelocity()
	{
		Vector3 playerHorizentalvelocity = _stateMachine.Rigidbody.velocity;
		playerHorizentalvelocity.y = 0f;
		return playerHorizentalvelocity;
	}

	protected void MoveOnAnimation()
	{
		float animationSpeed = _stateMachine.Animator.deltaPosition.magnitude;

		// test
		Vector3 direction = _stateMachine.transform.forward.normalized;

		// ���ο� ���� ���
		Vector3 newDeltaPosition = direction * animationSpeed;

		_stateMachine.transform.Translate(newDeltaPosition);
	}

	protected Vector3 AdjustDirectionToSlope(Vector3 direction)
	{
		return Vector3.ProjectOnPlane(direction, _sloperHit.normal);
	}

	protected Vector3 AdjustKeyDirectionToSlope(Vector3 direction)
	{
		return AdjustDirectionToSlope(Vector3.ProjectOnPlane(direction, Vector3.Cross(_stateMachine.velocity.normalized, Vector3.up)));
	}

	/// <summary>
	/// �÷��̾ ���� ���� �ִ��� �˻��Ѵ�.
	/// </summary>
	/// <returns></returns>
	public bool IsOnSlope()
	{
		Ray ray = new Ray(_stateMachine.transform.position, Vector3.down);
		if (Physics.Raycast(ray, out _sloperHit, _rayDistance, _groundLayer))
		{
			var angle = Vector3.Angle(Vector3.up, _sloperHit.normal);
			return angle != 0f && angle < _maxSlopeAngle;
		}
		return false;
	}

	public void Float()
	{
		// ĸ���ݶ��̴� ��ġ�� �����´�.
		Vector3 capsuleColliderCenterInWorldSpace = _stateMachine.Player.capsuleColldierUtility.CapsuleColliderData.Collider.bounds.center;

		// ������ġ�� ĸ���ݶ��̴� ���ͷ� ��´�.
		Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

		// ���̸� ���� ĳ�����Ѵ�.
		if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _slopeData.FloatRayDistance, _stateMachine.Player.layerData.GroundLayer, QueryTriggerInteraction.Ignore))
		{
			float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

			SetSlopeSpeedModifierOnAngle(groundAngle);

			float distanceToFloatingPoint = _stateMachine.Player.capsuleColldierUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y
				* _stateMachine.Player.transform.localScale.y - hit.distance;

			if (distanceToFloatingPoint == 0f)
			{
				return;
			}

			// ĸ���� ����� �ϴ� velocity�� ����մϴ�.
			float amountToLift = distanceToFloatingPoint * _slopeData.StepReachForce - GetPlayerVerticalVelocity().y;

			// y���͸� �����մϴ�.
			Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

			// ĸ���� ���ϴ�.
			_stateMachine.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
		}
	}

	private void SetSlopeSpeedModifierOnAngle(float groundAngle)
	{
	}

	protected Vector3 GetPlayerVerticalVelocity()
	{
		return new Vector3(0f, _stateMachine.Rigidbody.velocity.y, 0f);
	}


}