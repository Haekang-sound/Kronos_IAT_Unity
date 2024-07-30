using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// 몬스터 셀렉터는 이제 사용하지 않는다
/// </summary>
public class MonsterSelector : MonoBehaviour
{

	// 몬스터를 관리할 목록
	List<GameObject> monsters;

	// 오토 타겟에 그걸 해주자
	public AutoTargetting autoTargetor;


	float? min = null;

	// Start is called before the first frame update
	void Start()
	{
		monsters = MonsterManager.Instance.list;
		
	}


    public bool FindTarget()
    {
        // 몬스터리스트가 없거나 사이즈가 0이라면 false
        if (monsters == null || monsters.Count == 0)
        {
            return false;
        }
        // 현재 몬스터 목록을 순회한다.
        for (int i = 0; i < monsters.Count; i++)
        {
            // 몬스터와 플레이어 사이의 거리벡터의 크기를 구한다.
            /// 몬스터의 트랜스폼을 
            /// 매번 컴포넌트 접근으로 찾아야하는 점은 
            /// 너무 아쉬운 일이다. 방법을 찾아볼까?
            /// 1. findTarget기능의 위치를 바꾼다.
            float value = Mathf.Abs((autoTargetor.PlayerObject.position - monsters[i].GetComponent<Transform>().position).magnitude);

            // 현재 min값보다 value가 작다면 혹은 min에 값이 들어있지 않다면 
            if (min > value || min == null)
            {
                // min 값을 교체하고 
                min = value;

                // 가장 작은 값을 가진 트랜스폼을 타겟으로 설정한다.
                autoTargetor.Target = monsters[i].GetComponent<Transform>();
            }
        }

        min = null;
        return true;
    }
}
