using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potal : MonoBehaviour
{

	public string NextScene;

	Transform cubeTM;
	float rotSpeed = 50f;
	// Start is called before the first frame update
	void Start()
	{
		cubeTM = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update()
	{
		// ����.. �������ϱ�
		cubeTM.Rotate(0f, rotSpeed * Time.deltaTime, 0f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.CompareTag("Player"))
		{
			SceneManager.LoadScene(NextScene);
			Debug.Log("���� �ǰ���");
		}
	}

}
