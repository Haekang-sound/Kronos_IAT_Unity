using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Rendering;
using UnityEngine;

public class MonsterSelector : MonoBehaviour
{

	public Transform Monsetr0;
	public Transform Monsetr1;
	public Transform Monsetr2;
	public Transform Monsetr3;

	// ���͸� ������ ���
	List<Transform> monsters = new List<Transform>();

	// ���� Ÿ�ٿ� �װ� ������
	public AutoTargetting autoTargetor;


	float? min = null;
	int indexNum;


	// Start is called before the first frame update
	void Start()
	{
		// �ϴ� ���
		monsters.Add(Monsetr0);
		monsters.Add(Monsetr1);
		monsters.Add(Monsetr2);
		monsters.Add(Monsetr3);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			FindTarget();

		}
	}

	public void FindTarget()
	{
		// ���� ���� ����� ��ȸ�Ѵ�.
		for (int i = 0; i < monsters.Count; i++)
		{
			// ���Ϳ� �÷��̾� ������ �Ÿ������� ũ�⸦ ���Ѵ�.
			float value = Mathf.Abs((autoTargetor.PlayerObject.position - monsters[i].position).magnitude);

			// ���� min������ value�� �۴ٸ� Ȥ�� min�� ���� ������� �ʴٸ� 
			if (min > value || min == null)
			{
				// min ���� ��ü�ϰ� 
				/// �����߰�, �� �� min���� ��ϵ� ���� ���� �ٲ��� �ʴ´�.
				min = value;
				// ���� ���� ���� ���� Ʈ�������� Ÿ������ �����Ѵ�.
				autoTargetor.Target = monsters[i];
			}
		}

		min = null;
	}
}
