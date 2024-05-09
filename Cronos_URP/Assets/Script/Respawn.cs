using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �÷��̾ �������ϴ� ģ�� �ӽ÷� ����
public class Respawn : MonoBehaviour
{
	GameObject playerObj;
	Player player;

	private void Awake()
	{
		//DontDestroyOnLoad(this.gameObject);
	}
	private void Start()
	{
		//0. �÷��̾ �ʿ��ϴ�.
		playerObj = GameObject.Find("Player");
		player = playerObj.GetComponent<Player>();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			RespawnPlayer();
		}
	}
	public void RespawnPlayer()
	{
		//1. �÷��̾ inactive �Ѵ�.
		playerObj.SetActive(false);
		//2. �÷��̾��� �����͸� �������.
		player.PlayerRespawn();
		//3. �÷��̾ active �Ѵ�.
		playerObj.SetActive(true);
	}
}
