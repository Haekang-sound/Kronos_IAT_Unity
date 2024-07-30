using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> objects = new Dictionary<Type, UnityEngine.Object[]>();

    protected enum Buttons
    {
        Confirm
    }

    protected enum Texts
    {
        UI_TP,
        UI_CP,
        InteractionText,
        BronzeTitle,
        SilverTitle,
        GoldTitle,
        BronzeText,
        SilverText,
        GoldText,
        ConfirmText
    }

    protected enum Images
    {
        InteractionImage,
        BronzeImage,
        SilverImage,
        GoldImage
    }

    protected enum GameObjects
    {
        BronzeUp,
        SilverUp,
        GoldUp
    }

    public GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform result = FindChild<Transform>(go, name, recursive);
        if (result != null)
            return result.gameObject;

        return null;
    }

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
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];
        objects.Add(typeof(T), objs);

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                objs[i] = FindChild(gameObject, names[i], true);

            else
                objs[i] = FindChild<T>(gameObject, names[i], true);

            if (objs[i] == null)
                Debug.Log($"Failed to Bind {names[i]}");
        }
    }

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objs = null;
        if (objects.TryGetValue(typeof(T), out objs) == false)
            return null;

        return objs[idx] as T;
    }

    protected TextMeshProUGUI GetText(int idx)
    {
        return Get<TextMeshProUGUI>(idx);
    }

    protected Button GetButton(int idx)
    {
        return Get<Button>(idx);
    }

    protected Image GetImage(int idx) { return Get<Image>(idx); }
}
