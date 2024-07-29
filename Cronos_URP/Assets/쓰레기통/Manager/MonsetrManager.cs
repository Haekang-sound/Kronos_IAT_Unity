using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;


/// <summary>
/// ICollection을 이용해서 매니저를 만들어보자
/// 편의상 GameObject로 만들었지만
/// 나중에는 Monster 혹은 그런 계통의 Prefab의 생성,삭제를 관리해야 할 듯
/// </summary>
class MonsterManager : MonoBehaviour
{
    float? min = null;
    //public AutoTargetting autoTargetor; // 매니저가 이 컴포넌트를 알 이유는 없다

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
    // 몬스터 자료형을 받아야하니까
    // 프리펩을 받을 수 있게 열어놓는 무언가.
    public GameObject monster;
    public GameObject Player;
    public List<GameObject> list = new List<GameObject>();

    private MonsterManager() {  }

    private void Update()
    {
        /// 임시함수
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
    /// 몬스터를 생성하는 함수
    /// </summary>
    public void CreateMonster()
    {
        // 몬스터를 생성한다.
        GameObject temp = Instantiate(monster, new Vector3(10, 0, 5), Quaternion.identity);
        // 몬스터를 리스트에 저장한다.
        Add(temp);
        Debug.Log($"현재 몬스터 수 {list.Count}");
    }

	public void CreatePlayer()
	{
		// 플레이어를 생성한다
		GameObject player = Instantiate(Player, new Vector3(0, 10, 5), Quaternion.identity);
		player.GetComponent<Player>().PlayerRespawn();


	}

	/// <summary>
	/// 몬스터를 삭제하는 함수
	/// </summary>
	public void DeleteMonster()
    {
        if (list.Count < 1) { return; }
        GameObject temp = list[(list.Count - 1)];
        Remove(list[(list.Count - 1)]);
        Destroy(temp);

    }

    /// <summary>
    /// 가장 가까운 몬스터를 찾아주자
    /// </summary>
    /// <returns>성공여부를 리턴한다</returns>
    public Transform FindTarget(Vector3 playerPos)
    {
        Transform target = null;
        // 몬스터리스트가 없거나 사이즈가 0이라면 false
        if (list.Count == 0)
        {
            return null;
        }
        // 현재 몬스터 목록을 순회한다.
        for (int i = 0; i < list.Count; i++)
        {
            // 몬스터와 플레이어 사이의 거리벡터의 크기를 구한다.
            float value = Mathf.Abs((playerPos - list[i].GetComponent<Transform>().position).magnitude);

            // 현재 min값보다 value가 작다면 혹은 min에 값이 들어있지 않다면 
            if (min > value || min == null)
            {
                // min 값을 교체하고 
                min = value;

                // 가장 작은 값을 가진 트랜스폼을 타겟으로 설정한다.
                target = list[i].GetComponent<Transform>();
            }
        }

        min = null;
        return target;
    }

    public int Count { get { return list.Count; } }
    public bool IsReadOnly { get { return false; } }



    public void Clear() { list = new List<GameObject>(); } // C#의 초기화 ... 미쳤다..
    public void Add(GameObject gameObject)
    {
        list.Add(gameObject);
    }
    public bool Remove(GameObject gameObject)
    {
        Debug.Log($"현재 몬스터 수 {list.Count}");
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
