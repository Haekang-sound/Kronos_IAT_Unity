using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameObjectDestroyer : MonoBehaviour
{
    public void DestroyGameObject()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            DestroyImmediate(gameObject);
        }
        else
#endif
        {
            Destroy(gameObject);
        }
    }
}
