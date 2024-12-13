using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ݶ��̴��� ������ �������ų� �������� �߷��� 
public class GravityField : MonoBehaviour
{
	bool isSlow;
	public bool isGravitation;
	public float AnimSpeed = 0.1f;
	public float Gravity = 3f;

	Animator _animator;
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag.ToString());
		//�÷��̾���
		if (other.CompareTag("Player"))
		{
			isSlow = true;
			Debug.Log("���� �÷��̾��! �ִϸ����͵� �־��!");

		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isSlow = false;
			PlayerStateMachine.GetInstance().Animator.speed = 1f;
			Debug.Log("�߳�� ���ϴ�");

		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isSlow)
		{
			Debug.Log("�������ٴ·�����");
			PlayerStateMachine.GetInstance().Animator.speed = AnimSpeed;

			Debug.Log(PlayerStateMachine.GetInstance().Animator.speed);
		}



	}
	private void FixedUpdate()
	{
		if (isGravitation && isSlow)
		{
			Vector3 temp = transform.position - Player.Instance.transform.position;
			temp.y = 0f;

			// �Ÿ��� ����ؼ� ��¼����¼���ϰ� ������ �ҵ�
			// �����̶� �ұ�? 
			PlayerStateMachine.GetInstance().Rigidbody.velocity += temp.normalized * Gravity;
		}
	}

	private void OnDestroy()
	{
		if (PlayerStateMachine.GetInstance() != null)
			PlayerStateMachine.GetInstance().Animator.speed = 1f;
	}

	public void SetAnimSpeed(float value)
	{

	}
}
