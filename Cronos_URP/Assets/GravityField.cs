using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 콜라이더에 닿으면 느려지거나 빨라지는 중력장 
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
		//플레이어라면
		if (other.CompareTag("Player"))
		{
			isSlow = true;
			Debug.Log("저는 플레이어에용! 애니메이터도 있어용!");

		}
	}
	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			isSlow = false;
			PlayerStateMachine.GetInstance().Animator.speed = 1f;
			Debug.Log("잘놀다 갑니다");

		}
	}

	// Update is called once per frame
	void Update()
	{
		if (isSlow)
		{
			Debug.Log("느려진다는려진다");
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

			// 거리에 비례해서 어쩌구저쩌구하게 만들어야 할듯
			// 폴리싱때 할까? 
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
