using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// 바인딩할 UI요소들이 상속받아야 할 최상위 UI 클래스입니다.
/// 재귀 탐색을 통해 UI 요소의 타입과 맞는 오브젝트를 딕셔너리에 담고
/// 하위 클래스에서 사용할 함수들을 구현해놨습니다.
/// </summary>
public class UI_Bind : MonoBehaviour
{
    /// <summary>
    /// UI 타입에 맞는 오브젝트를 담기 위한 딕셔너리입니다.
    /// </summary>
    protected Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// 바인딩할 UI 요소를 정리한 enum입니다. UI가 추가될수록 멤버가 증가합니다.
    /// 현재는 TP 텍스트 하나만 사용합니다.
    /// </summary>
    protected enum Texts
    {
        HUD_TPAmount,
    }

    /// T 타입이 아닌, 게임오브젝트를 바로 찾기 위해 별도로 구현한 함수입니다.
    public GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform result = FindChild<Transform>(go, name, recursive);
        if (result != null)
            return result.gameObject;

        return null;
    }

    /// 상위 UI에서 자식 오브젝트를 타고 내려가기 위해 재귀적으로 탐색하는 함수입니다.
    public T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }

            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                // 찾는 오브젝트의 이름이 null이라면 일단 T타입을 반환한다.
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    /// Find함수로 발견한 요소와 딕셔너리를 연결하는 함수입니다.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // enum의 type을 저장하는 로컬 배열
        string[] names = Enum.GetNames(type);

        // 배열의 멤버만큼의 유니티 오브젝트 로컬 배열
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objs[i] = FindChild(gameObject, names[i], true);

            else
                objs[i] = FindChild<T>(gameObject, names[i], true);

            // 찾는 이름의 UI가 없다면
            if (objs[i] == null)
                Debug.Log($"Failed to Bind {names[i]}");
        }
    }

    /// TMP만을 enum 타입으로 사용하기 때문에 별도의 Getter를 구현했습니다.
    protected TextMeshProUGUI GetText(int idx)
    {
        return Get<TextMeshProUGUI>(idx);
    }

    /// 딕셔너리에 바인딩한 UI 요소를 Get하는 함수입니다.
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs = null;
        if (objects.TryGetValue(typeof(T), out objs) == false)
            return null;

        return objs[idx] as T;
    }
}
