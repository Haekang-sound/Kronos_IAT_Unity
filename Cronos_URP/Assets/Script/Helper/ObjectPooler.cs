using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 제네릭 오브젝트 풀 클래스로, 주어진 타입 T의 MonoBehaviour 객체를 관리합니다.
/// </summary>
public class ObjectPooler<T> where T : UnityEngine.MonoBehaviour, IPooled<T>
{
    public T[] instances;

    protected Stack<int> m_FreeIdx;

    public void Initialize(int count, T prefab)
    {
        instances = new T[count];
        m_FreeIdx = new Stack<int>(count);

        for (int i = 0; i < count; ++i)
        {
            instances[i] = Object.Instantiate(prefab);
            instances[i].gameObject.SetActive(false);
            instances[i].poolID = i;
            instances[i].pool = this;

            m_FreeIdx.Push(i);
        }
    }

    /// <summary>
    /// 객체 풀에서 비활성화된 객체를 하나 가져와 활성화 상태로 반환합니다.
    /// </summary>
    /// <returns>활성화된 객체</returns>
    public T GetNew()
    {
        int idx = m_FreeIdx.Pop();
        instances[idx].gameObject.SetActive(true);

        return instances[idx];
    }

    /// <summary>
    /// 사용이 끝난 객체를 풀에 반환하여 비활성화합니다.
    /// </summary>
    /// <param name="obj">풀에 반환할 객체</param>

    public void Free(T obj)
    {
        m_FreeIdx.Push(obj.poolID);
        instances[obj.poolID].gameObject.SetActive(false);
    }
}

/// <summary>
/// 객체 풀에서 관리될 객체가 구현해야 하는 인터페이스입니다.
/// </summary>
/// <typeparam name="T">풀에서 관리할 객체의 타입</typeparam>
public interface IPooled<T> where T : MonoBehaviour, IPooled<T>
{
    int poolID { get; set; }
    ObjectPooler<T> pool { get; set; }
}