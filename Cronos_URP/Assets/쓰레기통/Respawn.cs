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
	}
	private void Start()
	{
		//0. �÷��̾ �ʿ��ϴ�.
		//playerObj = GameObject.Find("Player");
		//player = playerObj.GetComponentInChildren<Player>();
		
    }

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			//GameManager.Instance.isRespawn = true;
            RespawnPlayer();
		}
	}
	public void RespawnPlayer()
	{
		//1. �÷��̾ inactive �Ѵ�.
		gameObject.SetActive(false);
		//2. �÷��̾��� �����͸� �������.
		//GameObject.FindObjectsOfType<Player>()[0].PlayerRespawn();
        //3. �÷��̾ active �Ѵ�.
        gameObject.SetActive(true);
	}
}
