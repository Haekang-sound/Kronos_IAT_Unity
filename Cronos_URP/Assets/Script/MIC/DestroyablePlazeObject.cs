using UnityEngine;
using System;

public class DestroyablePlazeObject : MonoBehaviour
{
    public event Action OnDestroyed;


    public void Destroy()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
