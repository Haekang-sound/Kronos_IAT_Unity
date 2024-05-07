using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Potal : MonoBehaviour
{
	Transform cubeTM;
	float rotSpeed = 50f;
	// Start is called before the first frame update
	void Start()
	{
		cubeTM = GetComponentInChildren<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		// ����.. �������ϱ�
		cubeTM.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{

			Debug.Log("�� �¾ҳ�");
		}

	}
}
