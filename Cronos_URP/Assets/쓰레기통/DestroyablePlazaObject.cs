using UnityEngine;
using System;


/// <summary>
/// 파괴되는 분수 오브젝트 클래스
/// 하는 역할은 Destroy 되었을 때 삼각형 인디케이터도 Destroy
/// </summary>
public class DestroyablePlazaObject : MonoBehaviour
{
    public event Action OnDestroyed;
    public GameObject triangle;

    //public void Destroy()
    //{
    //    Destroy(triangle);
    //}

    //void OnDestroy()
    //{
    //    OnDestroyed?.Invoke();
    //}
}
