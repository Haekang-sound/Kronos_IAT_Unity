using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// ���ε��� UI��ҵ��� ��ӹ޾ƾ� �� �ֻ��� UI Ŭ�����Դϴ�.
/// ��� Ž���� ���� UI ����� Ÿ�԰� �´� ������Ʈ�� ��ųʸ��� ���
/// ���� Ŭ�������� ����� �Լ����� �����س����ϴ�.
/// </summary>
public class UI_Bind : MonoBehaviour
{
    /// <summary>
    /// UI Ÿ�Կ� �´� ������Ʈ�� ��� ���� ��ųʸ��Դϴ�.
    /// </summary>
    protected Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// ���ε��� UI ��Ҹ� ������ enum�Դϴ�. UI�� �߰��ɼ��� ����� �����մϴ�.
    /// ����� TP �ؽ�Ʈ �ϳ��� ����մϴ�.
    /// </summary>
    protected enum Texts
    {
        HUD_TPAmount,
    }

    /// T Ÿ���� �ƴ�, ���ӿ�����Ʈ�� �ٷ� ã�� ���� ������ ������ �Լ��Դϴ�.
    public GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform result = FindChild<Transform>(go, name, recursive);
        if (result != null)
            return result.gameObject;

        return null;
    }

    /// ���� UI���� �ڽ� ������Ʈ�� Ÿ�� �������� ���� ��������� Ž���ϴ� �Լ��Դϴ�.
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
                // ã�� ������Ʈ�� �̸��� null�̶�� �ϴ� TŸ���� ��ȯ�Ѵ�.
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    /// Find�Լ��� �߰��� ��ҿ� ��ųʸ��� �����ϴ� �Լ��Դϴ�.
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        // enum�� type�� �����ϴ� ���� �迭
        string[] names = Enum.GetNames(type);

        // �迭�� �����ŭ�� ����Ƽ ������Ʈ ���� �迭
        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objs[i] = FindChild(gameObject, names[i], true);

            else
                objs[i] = FindChild<T>(gameObject, names[i], true);

            // ã�� �̸��� UI�� ���ٸ�
            if (objs[i] == null)
                Debug.Log($"Failed to Bind {names[i]}");
        }
    }

    /// TMP���� enum Ÿ������ ����ϱ� ������ ������ Getter�� �����߽��ϴ�.
    protected TextMeshProUGUI GetText(int idx)
    {
        return Get<TextMeshProUGUI>(idx);
    }

    /// ��ųʸ��� ���ε��� UI ��Ҹ� Get�ϴ� �Լ��Դϴ�.
    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs = null;
        if (objects.TryGetValue(typeof(T), out objs) == false)
            return null;

        return objs[idx] as T;
    }
}
