using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 플레이어를 리스폰하는 친구 임시로 만듦
public class Respawn : MonoBehaviour
{
	GameObject playerObj;
	Player player;

	private void Awake()
	{
	}
	private void Start()
	{
		//0. 플레이어가 필요하다.
		//playerObj = GameObject.Find("Player");
		//player = playerObj.GetComponentInChildren<Player>();
		
    }

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			GameManager.Instance.isRespawn = true;
            RespawnPlayer();
		}
	}
	public void RespawnPlayer()
	{
		//1. 플레이어를 inactive 한다.
		gameObject.SetActive(false);
		//2. 플레이어의 데이터를 덧씌운다.
		GameObject.FindObjectsOfType<Player>()[0].PlayerRespawn();
        //3. 플레이어를 active 한다.
        gameObject.SetActive(true);
	}
}
