using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 체크포인트는 활성, 비활성 상태가 있다.
// 한번 활성화된 체크포인트는 다시 사용되지 않는다.
// 체크포인트는 플레이어의 데이터를 조작한다?
// 아니 플레이어 내부의 체크포인트 함수를 실행시키자
public class CheckPoint : MonoBehaviour
{

	//bool isOn = false;

	Transform cubeTM;

	float rotSpeed = 50f;
	void Start()
	{
		cubeTM = GetComponent<Transform>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			// 충돌체가 플레이어일 경우에 플레이어 데이터를 저장한다.
			other.gameObject.GetComponent<Player>().SavePlayerData();
			//isOn = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// 돌면.. 멋있으니까
		cubeTM.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
	}

}
