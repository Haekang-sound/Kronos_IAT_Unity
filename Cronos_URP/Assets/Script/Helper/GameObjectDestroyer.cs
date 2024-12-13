using UnityEngine;

/// <summary>
/// 게임 오브젝트를 삭제하는 기능을 제공하는 클래스입니다.
/// 게임이 실행 중인지 여부에 따라 삭제 방법을 다르게 처리합니다.
/// </summary>
public class GameObjectDestroyer : MonoBehaviour
{
    /// <summary>
    /// 게임 오브젝트를 삭제합니다.
    /// 에디터에서는 실행 중이지 않을 때 즉시 삭제하고, 게임 실행 중일 때는 정상적으로 삭제합니다.
    /// </summary>
    public void DestroyGameObject()
    {
#if UNITY_EDITOR
        // 에디터에서 실행 중이지 않으면 즉시 오브젝트를 삭제
        if (!Application.isPlaying)
        {
            DestroyImmediate(gameObject);
        }
        else
#endif
        {
            // 게임 실행 중일 때는 일반적으로 삭제
            Destroy(gameObject);
        }
    }
}
