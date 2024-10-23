using System;
using System.Globalization;
using System.Resources;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public abstract class PlayerBaseState : State
{
	// 변수를 읽기전용으로 선언
	protected readonly PlayerStateMachine stateMachine;
	Vector3 moveDirection;
	private SlopeData slopeData;

	protected PlayerBaseState(PlayerStateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
		slopeData = stateMachine.Player.CapsuleColldierUtility.SlopeData;
	}



	/// <summary>
	/// 카메라 정보를 기반으로
	/// Player의 이동방향을 구한다.
	/// </summary>
	protected void CalculateMoveDirection()
	{
		// 스테이트 머신에 들어있는 카메라 정보를 기반으로
		// 카메라의 전방, 좌우 벡터를 저장한다.
		Vector3 cameraForward = new(stateMachine.MainCamera.forward.x, 0, stateMachine.MainCamera.forward.z);
		Vector3 cameraRight = new(stateMachine.MainCamera.right.x, 0, stateMachine.MainCamera.right.z);

		// 이동벡터생성,
		// 카메라의 전방벡터에 인풋의 move.y 수치를 곱한다,
		// 카메라의 좌우벡터에 인풋의 movecomposite.x를 곱한다.
		moveDirection = cameraForward.normalized * stateMachine.InputReader.moveComposite.y // 전방
								+ cameraRight.normalized * stateMachine.InputReader.moveComposite.x;    // 후방

		// 상태머신의 속도는 이동벡터와 속력의 곱이다.
		stateMachine.Velocity.x = moveDirection.x * stateMachine.Player.moveSpeed;
		stateMachine.Velocity.y = 0f;
		stateMachine.Velocity.z = moveDirection.z * stateMachine.Player.moveSpeed;
	}

	/// <summary>
	/// 플레이어를 이동방향으로 회전시킨다.
	/// </summary>
	protected void FaceMoveDirection()
	{
		Vector3 faceDirection = new(stateMachine.Velocity.x, 0f, stateMachine.Velocity.z);

		// 이동속도가 없다면
		if (faceDirection == Vector3.zero)
		{
			// 아무것도 하지 않겠다.
			return;
		}
		// 플레이어의 회전은 구면 선형보간의 형태로 이루어진다. 
		stateMachine.Rigidbody.MoveRotation(Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(faceDirection), stateMachine.Player.lookRotationDampFactor * Time.fixedDeltaTime));

	}

	/// <summary>
	/// Player가 갖고 있는 데이터를 종합해서
	/// Player를 이동시킨다
	/// </summary>
	protected void Move()
	{

// 		bool isOnSlope = IsOnSlope();
// 		if (isOnSlope)
// 		{
// 			stateMachine.Rigidbody.useGravity = false;
// 		}
// 		else
// 		{
// 			stateMachine.Rigidbody.useGravity = true;
// 		}
		Vector3 velocity = /*isOnSlope ? AdjustDirectionToSlope(stateMachine.Velocity) :*/ stateMachine.Velocity;//.normalized;
		Vector3 gravity = /*isOnSlope ? Vector3.zero :*/ Vector3.down * Mathf.Abs(stateMachine.Rigidbody.velocity.y);

		stateMachine.Rigidbody.velocity = velocity * Time.fixedDeltaTime * stateMachine.Player.moveSpeed
			 * stateMachine.Animator.speed + gravity;

		// 		stateMachine.Rigidbody.velocity = /*velocity*/ AdjustKeyDirectionToSlope(stateMachine.Animator.deltaPosition)/** Time.fixedDeltaTime * stateMachine.Player.moveSpeed*/
		// 	  * stateMachine.Animator.speed + gravity;

		//Float();

	}

	protected void Move(Vector3 moveVector)
	{

// 		bool isOnSlope = IsOnSlope();
// 		if (isOnSlope)
// 		{
// 			stateMachine.Rigidbody.useGravity = false;
// 		}
// 		else
// 		{
// 			stateMachine.Rigidbody.useGravity = true;
// 		}
		Vector3 velocity = moveVector;// AdjustKeyDirectionToSlope( moveVector);
		Vector3 gravity = /*isOnSlope ? Vector3.zero : */Vector3.down * /*Mathf.Abs(stateMachine.Rigidbody.velocity.y)*/9.81f;

		//stateMachine.Rigidbody.velocity = new Vector3(10, 0, 0);
		stateMachine.Rigidbody.velocity = stateMachine.Animator.deltaPosition + gravity; ;//velocity;// * stateMachine.Animator.speed ;// * Time.fixedDeltaTime
// 		Debug.Log(stateMachine.Rigidbody.velocity);
		/** stateMachine.Animator.speed + gravity;*/

		Float();

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

		// 새로운 벡터 계산
		Vector3 newDeltaPosition = direction * animationSpeed;

		stateMachine.transform.Translate(newDeltaPosition);
	}


	private const float ray_distance = 2f;
	private RaycastHit sloperHit;
	private int groundLayer = 1 << LayerMask.NameToLayer("Ground");
	private float maxSlopeAngle = 30f;


	protected Vector3 AdjustDirectionToSlope(Vector3 direction)
	{
		return Vector3.ProjectOnPlane(direction, sloperHit.normal);
	}

	protected Vector3 AdjustKeyDirectionToSlope(Vector3 direction)
	{
		return AdjustDirectionToSlope(Vector3.ProjectOnPlane(direction, Vector3.Cross(stateMachine.Velocity.normalized, Vector3.up)));
	}

	public bool IsOnSlope()
	{
		Ray ray = new Ray(stateMachine.transform.position, Vector3.down);
		if (Physics.Raycast(ray, out sloperHit, ray_distance, groundLayer))
		{
			var angle = Vector3.Angle(Vector3.up, sloperHit.normal);
			return angle != 0f && angle < maxSlopeAngle;
		}
		return false;
	}

	public void Float()
	{
		// 캡슐콜라이더 위치를 가져온다.
		Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.CapsuleColldierUtility.CapsuleColliderData.Collider.bounds.center;

		// 레이위치를 캡슐콜라이더 센터로 잡는다.
		Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

		// 레이를 쏴서 캐스팅한다.
		if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
		{
			float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

			SetSlopeSpeedModifierOnAngle(groundAngle);

			float distanceToFloatingPoint = stateMachine.Player.CapsuleColldierUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y
				* stateMachine.Player.transform.localScale.y - hit.distance;

			if (distanceToFloatingPoint == 0f)
			{
				return;
			}
			// 캡슐을 띄워야 하는 velocity를 계산합니다.
			float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;
			// y벡터를 생성합니다.
			Vector3 liftForce = new Vector3(0f, amountToLift, 0f);
			// 캡슐을 띄웁니다.
			stateMachine.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
		}
	}

	private void SetSlopeSpeedModifierOnAngle(float groundAngle)
	{
	}
	protected Vector3 GetPlayerVerticalVelocity()
	{
		return new Vector3(0f, stateMachine.Rigidbody.velocity.y, 0f);
	}


}