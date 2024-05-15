using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.PackageManager;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// ICollection�� �̿��ؼ� �Ŵ����� ������
/// ���ǻ� GameObject�� ���������
/// ���߿��� Monster Ȥ�� �׷� ������ Prefab�� ����,������ �����ؾ� �� ��
/// </summary>
class MonsterManager : MonoBehaviour
{
    float? min = null;
    //public AutoTargetting autoTargetor; // �Ŵ����� �� ������Ʈ�� �� ������ ����

    private static MonsterManager _instance;

    public static MonsterManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MonsterManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("MonsterManager");
                    _instance = go.AddComponent<MonsterManager>();
                }
            }
            return _instance;
        }
    }
    // ���� �ڷ����� �޾ƾ��ϴϱ�
    // �������� ���� �� �ְ� ������� ����.
    public GameObject monster;
    public GameObject Player;
    public List<GameObject> list = new List<GameObject>();

    private MonsterManager() {  }

    private void Update()
    {
        /// �ӽ��Լ�
        if (Input.GetKeyUp(KeyCode.Equals))
        {
            CreateMonster();
        }
        if (Input.GetKeyUp(KeyCode.Backspace))
        {
            DeleteMonster();
        }

		if (Input.GetKeyUp(KeyCode.Alpha0))
		{
			CreatePlayer();
		}
	}


    /// <summary>
    /// ���͸� �����ϴ� �Լ�
    /// </summary>
    public void CreateMonster()
    {
        // ���͸� �����Ѵ�.
        GameObject temp = Instantiate(monster, new Vector3(10, 0, 5), Quaternion.identity);
        // ���͸� ����Ʈ�� �����Ѵ�.
        Add(temp);
        Debug.Log($"���� ���� �� {list.Count}");
    }

	public void CreatePlayer()
	{
		// �÷��̾ �����Ѵ�
		GameObject player = Instantiate(Player, new Vector3(0, 10, 5), Quaternion.identity);
		player.GetComponent<Player>().PlayerRespawn();


	}

	/// <summary>
	/// ���͸� �����ϴ� �Լ�
	/// </summary>
	public void DeleteMonster()
    {
        if (list.Count < 1) { return; }
        GameObject temp = list[(list.Count - 1)];
        Remove(list[(list.Count - 1)]);
        Destroy(temp);

    }

    /// <summary>
    /// ���� ����� ���͸� ã������
    /// </summary>
    /// <returns>�������θ� �����Ѵ�</returns>
    public Transform FindTarget(Vector3 playerPos)
    {
        Transform target = null;
        // ���͸���Ʈ�� ���ų� ����� 0�̶�� false
        if (list.Count == 0)
        {
            return null;
        }
        // ���� ���� ����� ��ȸ�Ѵ�.
        for (int i = 0; i < list.Count; i++)
        {
            // ���Ϳ� �÷��̾� ������ �Ÿ������� ũ�⸦ ���Ѵ�.
            float value = Mathf.Abs((playerPos - list[i].GetComponent<Transform>().position).magnitude);

            // ���� min������ value�� �۴ٸ� Ȥ�� min�� ���� ������� �ʴٸ� 
            if (min > value || min == null)
            {
                // min ���� ��ü�ϰ� 
                min = value;

                // ���� ���� ���� ���� Ʈ�������� Ÿ������ �����Ѵ�.
                target = list[i].GetComponent<Transform>();
            }
        }

        min = null;
        return target;
    }

    public int Count { get { return list.Count; } }
    public bool IsReadOnly { get { return false; } }



    public void Clear() { list = new List<GameObject>(); } // C#�� �ʱ�ȭ ... ���ƴ�..
    public void Add(GameObject gameObject)
    {
        list.Add(gameObject);
    }
    public bool Remove(GameObject gameObject)
    {
        Debug.Log($"���� ���� �� {list.Count}");
        return list.Remove(gameObject);
    }
    public bool Contains(GameObject gameObject) { return list.Contains(gameObject); }
    public void CopyTo(GameObject[] gameObject, int num) { list.CopyTo(gameObject, num); }


    public IEnumerator<GameObject> GetEnumerator()
    {
        return list.GetEnumerator();
    }

    //     IEnumerator IEnumerable.GetEnumerator()
    //     {
    //         return this.GetEnumerator();
    //     }



}
