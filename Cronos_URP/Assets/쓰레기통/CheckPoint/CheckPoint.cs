using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// üũ����Ʈ�� Ȱ��, ��Ȱ�� ���°� �ִ�.
// �ѹ� Ȱ��ȭ�� üũ����Ʈ�� �ٽ� ������ �ʴ´�.
// üũ����Ʈ�� �÷��̾��� �����͸� �����Ѵ�?
// �ƴ� �÷��̾� ������ üũ����Ʈ �Լ��� �����Ű��
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
			// �浹ü�� �÷��̾��� ��쿡 �÷��̾� �����͸� �����Ѵ�.
			other.gameObject.GetComponent<Player>().SavePlayerData();
			//isOn = true;
		}
	}

	// Update is called once per frame
	void Update()
	{
		// ����.. �������ϱ�
		cubeTM.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
	}

}
