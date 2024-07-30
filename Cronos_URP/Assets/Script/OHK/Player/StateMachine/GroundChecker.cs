using UnityEngine;

public class GroundChecker : MonoBehaviour
{
	[SerializeField] private LayerMask groundLayer;
	[SerializeField] private float radius = 0.3f;
	[SerializeField] private float offset = 0.1f;
	[SerializeField] private bool drawGizmo;

	private readonly int fallHash = Animator.StringToHash("isFalling");

	private void OnDrawGizmos()
	{
		if (!drawGizmo)
		{
			return;
		}

		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(transform.position + Vector3.up * offset, radius);
	}
	private void Update()
	{
		if(!IsGrounded())
		{
			PlayerStateMachine.GetInstance().Animator.SetBool(fallHash, true);
		}
	}

	public bool IsGrounded()
	{
		Vector3 pos = transform.position + Vector3.up * offset;
		bool isGrounded = Physics.CheckSphere(pos, radius, groundLayer);

		return isGrounded;
	}
}