using UnityEngine;

/// <summary>
/// 지면을 확인하는
/// 컴포넌트 클래스
/// 
/// ohk    v1
/// </summary>
public class GroundChecker : MonoBehaviour
{
	[SerializeField] private LayerMask _groundLayer;
	[SerializeField] private float _radius = 0.3f;
	[SerializeField] private float _offset = 0.1f;
	[SerializeField] private bool _drawGizmo;

	public bool toggleChecker = true;
	
	private void Update()
	{
		if (!IsGrounded() && toggleChecker)
		{
			PlayerStateMachine.GetInstance().Animator.SetBool(PlayerHashSet.Instance.IsFalling, true);
		}
	}

	private void OnDrawGizmos()
	{
		if (!_drawGizmo)
			return;

		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(transform.position + Vector3.up * _offset, _radius);
	}

	public bool IsGrounded()
	{
		Vector3 pos = transform.position + Vector3.up * _offset;
		bool isGrounded = Physics.CheckSphere(pos, _radius, _groundLayer);

		return isGrounded;
	}
}