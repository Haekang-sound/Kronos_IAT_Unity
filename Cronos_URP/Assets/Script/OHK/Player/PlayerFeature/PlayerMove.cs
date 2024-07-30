using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	// 필요한 변수
	public float speed;
	float hAxis;
	float vAxis;
	bool walking;
	bool wRun;
	Transform PlayerCAMRoot;    // 카메라루트를 담을 ? 
	Transform Player;   // 카메라루트를 담을 ? 
	Animator m_animator;        // 애니메이터정보를 가져와서 반영하자

	Vector3 moveVec;
	Animator anim;

	float dTime;

    void Start()
	{
		anim = GetComponentInChildren<Animator>();
	}

	// Update is called once per frame
	void Update()
	{
		dTime = Time.deltaTime;
		hAxis = Input.GetAxisRaw("Horizontal"); // 이동방향을 가져온다.
		vAxis = Input.GetAxisRaw("Vertical");
		
		// 이동버튼이 눌렸다면
		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{
			anim.SetBool("isWalking", true);
			walking = true;
		}
		else
		{
			anim.SetBool("isWalking", false);
			walking = false;
		}

		// 구르기
		if (Input.GetButtonDown("Jump") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
		{
			anim.SetTrigger("Dodge");
		}
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
		{
			transform.position += transform.forward.normalized * speed * 1.5f * dTime;
		}

		// 공격모션
		if (Input.GetButtonDown("Punch") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
		{
			anim.SetTrigger("Punch");
		}

		if (Input.GetButtonDown("Hook") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Hook"))
		{
			anim.SetTrigger("Hook");
		}

		// 달리기 버튼이 눌렸다면
		if (Input.GetButton("Run"))
		{
			anim.SetBool("isRunning", true);
			wRun = true;
		}
		else
		{
			anim.SetBool("isRunning", false);
			wRun =false;
		}

		// 이동중이라면 
		if (walking) 
		{
			Debug.Log("앞으로");
			transform.position += transform.forward.normalized * (wRun == true ? speed * 3f : speed) * dTime; // 전진
		}
	}
}

