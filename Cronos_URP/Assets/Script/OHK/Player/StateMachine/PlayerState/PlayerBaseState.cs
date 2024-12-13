using System;
using System.Globalization;
using System.Resources;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 플레이어의 기본 상태를 묘사
/// 모든 상태의 기본이 되며 여기서부터 파생됨..
/// 
/// ohk    v1 v1
/// </summary>
public abstract class PlayerBaseState : State
{
	// 변수를 읽기전용으로 선언
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
	
	// 벨로시티가 잘못되면 리턴
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
	/// 카메라 정보를 기반으로
	/// Player의 이동방향을 구한다.
	/// </summary>
	protected void CalculateMoveDirection()
	{
		if (_stateMachine.InputReader == null)
			return;

		// 스테이트 머신에 들어있는 카메라 정보를 기반으로
		// 카메라의 전후, 좌우 벡터를 저장한다.
		Vector3 cameraForward = new(_stateMachine.MainCamera.forward.x, 0, _stateMachine.MainCamera.forward.z);
		Vector3 cameraRight = new(_stateMachine.MainCamera.right.x, 0, _stateMachine.MainCamera.right.z);

		// 이동벡터생성를 생성한다.
		_moveDirection = cameraForward.normalized * _stateMachine.InputReader.moveComposite.y 
								+ cameraRight.normalized * _stateMachine.InputReader.moveComposite.x;    

		// 상태머신의 속도는 이동벡터와 속력의 곱이다.
		_stateMachine.velocity.x = _moveDirection.x * _stateMachine.Player.moveSpeed;
		_stateMachine.velocity.y = 0f;
		_stateMachine.velocity.z = _moveDirection.z * _stateMachine.Player.moveSpeed;
	}

	/// <summary>
	/// 인풋키를 이용해 생성된 방향으로
	/// 플레이어를 회전시킨다.
	/// </summary>
	protected void FaceMoveDirection()
	{
		Vector3 faceDirection = new(_stateMachine.velocity.x, 0f, _stateMachine.velocity.z);

		// 이동속도가 없다면
		// 동작하지 않는다.
		if (faceDirection == Vector3.zero)
		{
			return;
		}

		// 플레이어의 회전은 구면 선형보간의 형태로 이루어진다. 
		_stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(_stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), _stateMachine.Player.lookRotationDampFactor * Time.fixedDeltaTime));

	}

	/// <summary>
	/// 직접 지정한 방향으로
	/// 플레이어를 회전시킨다.
	/// </summary>
	/// <param name="faceDir"></param>
	protected void FaceMoveDirection(Vector3 faceDir)
	{
		faceDir.y = 0f;

		// 이동속도가 없다면
		// 동작하지 않는다.
		if (faceDir == Vector3.zero)
		{
			return;
		}
		// 플레이어의 회전은 구면 선형보간의 형태로 이루어진다. 
		_stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(_stateMachine.transform.rotation, Quaternion.LookRotation(faceDir), _stateMachine.Player.lookRotationDampFactor * Time.fixedDeltaTime));
	}

	/// <summary>
	/// Player가 갖고 있는 데이터를 종합해서
	/// Player를 이동시킨다
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

		// 새로운 벡터 계산
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
	/// 플레이어가 경사면 윈에 있는지 검사한다.
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
		// 캡슐콜라이더 위치를 가져온다.
		Vector3 capsuleColliderCenterInWorldSpace = _stateMachine.Player.capsuleColldierUtility.CapsuleColliderData.Collider.bounds.center;

		// 레이위치를 캡슐콜라이더 센터로 잡는다.
		Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

		// 레이를 쏴서 캐스팅한다.
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

			// 캡슐을 띄워야 하는 velocity를 계산합니다.
			float amountToLift = distanceToFloatingPoint * _slopeData.StepReachForce - GetPlayerVerticalVelocity().y;

			// y벡터를 생성합니다.
			Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

			// 캡슐을 띄웁니다.
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