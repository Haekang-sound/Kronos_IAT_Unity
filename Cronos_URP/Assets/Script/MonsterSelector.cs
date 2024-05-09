using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// ���� �����ʹ� ���� ������� �ʴ´�
/// </summary>
public class MonsterSelector : MonoBehaviour
{

	// ���͸� ������ ���
	List<GameObject> monsters;

	// ���� Ÿ�ٿ� �װ� ������
	public AutoTargetting autoTargetor;


	float? min = null;

	// Start is called before the first frame update
	void Start()
	{
		monsters = MonsterManager.Instance.list;
		
	}

    public bool FindTarget()
    {
        // ���͸���Ʈ�� ���ų� ����� 0�̶�� false
        if (monsters == null || monsters.Count == 0)
        {
            return false;
        }
        // ���� ���� ����� ��ȸ�Ѵ�.
        for (int i = 0; i < monsters.Count; i++)
        {
            // ���Ϳ� �÷��̾� ������ �Ÿ������� ũ�⸦ ���Ѵ�.
            /// ������ Ʈ�������� 
            /// �Ź� ������Ʈ �������� ã�ƾ��ϴ� ���� 
            /// �ʹ� �ƽ��� ���̴�. ����� ã�ƺ���?
            /// 1. findTarget����� ��ġ�� �ٲ۴�.
            float value = Mathf.Abs((autoTargetor.PlayerObject.position - monsters[i].GetComponent<Transform>().position).magnitude);

            // ���� min������ value�� �۴ٸ� Ȥ�� min�� ���� ������� �ʴٸ� 
            if (min > value || min == null)
            {
                // min ���� ��ü�ϰ� 
                min = value;

                // ���� ���� ���� ���� Ʈ�������� Ÿ������ �����Ѵ�.
                autoTargetor.Target = monsters[i].GetComponent<Transform>();
            }
        }

        min = null;
        return true;
    }
}
