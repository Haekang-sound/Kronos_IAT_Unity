using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ݶ��̴��� ������ �������ų� �������� �߷��� 
public class GravityField : MonoBehaviour
{
	bool isSlow;
	public float AnimSpeed= 0.1f;
	Animator m_Animator;
	private void OnTriggerEnter(Collider other)
	{
		Debug.Log(other.tag.ToString());
		//�÷��̾���
		if (other.CompareTag("Player"))
		{
			isSlow = true;
			Debug.Log("���� �÷��̾��! �ִϸ����͵� �־��!");
			Player.Instance.SetSpeed(0f);

		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isSlow = false;
			PlayerStateMachine.GetInstance().Animator.speed = AnimSpeed;
			Debug.Log("�߳�� ���ϴ�");

		}
	}

	// Update is called once per frame
	void Update()
	{
		if(isSlow)
		{
			Debug.Log("�������ٴ·�����");
			PlayerStateMachine.GetInstance().Animator.speed = AnimSpeed;

			Debug.Log(PlayerStateMachine.GetInstance().Animator.speed);
			Player.Instance.SetSpeed(0f);
		}
	}

	private void OnDestroy()
	{
			PlayerStateMachine.GetInstance().Animator.speed = 1f;
	}

	public void SetAnimSpeed()
	{

	}
}
