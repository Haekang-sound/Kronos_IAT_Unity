using UnityEngine;
using System;

public class DestroyablePlazeObject : MonoBehaviour
{
    public event Action OnDestroyed;
    public GameObject indict;

    public void Destroy()
    {
        Destroy(indict);
    }

    void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
