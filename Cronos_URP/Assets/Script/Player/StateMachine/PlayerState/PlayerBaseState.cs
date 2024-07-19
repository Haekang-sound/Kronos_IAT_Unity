using System.Globalization;
using System.Resources;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public abstract class PlayerBaseState : State
{
	// 변수를 읽기전용으로 선언
	protected readonly PlayerStateMachine stateMachine;
	Vector3 moveDirection;

	protected PlayerBaseState(PlayerStateMachine stateMachine)
	{
		this.stateMachine = stateMachine;
	}

	/// <summary>
	/// 카메라 정보를 기반으로
	/// Player의 이동방향을 구한다.
	/// </summary>
	protected void CalculateMoveDirection ()
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
		stateMachine.Rigidbody.AddForce(stateMachine.Velocity
									* stateMachine.Animator.speed
									* stateMachine.Player.moveSpeed
									* Time.fixedDeltaTime
									- GetPlayerHorizentalVelocity(), ForceMode.VelocityChange);
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


	//public bool IsGrounded()
// 	{
// 		RaycastHit hit;
// 		float distance = 0.1f;
// 		bool isGrounded = Physics.Raycast(stateMachine.transform.position, Vector3.down, out hit, distance);
// 		//Debug.Log($"땅에 닿았나? {isGrounded}");
// 		// Raycast를 사용하여 땅을 체크
// 		stateMachine.IsGrounded = isGrounded;
// 		return isGrounded;
// 	}
// 	[SerializeField] private LayerMask groundLayer;
// 	[SerializeField] private float radius = 0.3f;
// 	[SerializeField] private float offset = 0.1f;
// 	[SerializeField] private bool drawGizmo;
// 
// 
// 	private void OnDrawGizmos()
// 	{
// 		if (!drawGizmo)
// 		{
// 			return;
// 		}
// 
// 		Gizmos.color = Color.cyan;
// 		Gizmos.DrawSphere(stateMachine.transform.position + Vector3.up * offset, radius);
// 	}
// 	public bool IsGrounded()
// 	{
// 		Vector3 pos = stateMachine.transform.position + Vector3.up * offset;
// 		bool isGrounded = Physics.CheckSphere(pos, radius, groundLayer);
// 
// 		return isGrounded;
// 	}



}