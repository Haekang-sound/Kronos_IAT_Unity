using UnityEngine;

/// <summary>
/// 플레이 동안 저장된 데이터를 호출하는 클래스입니다.
/// </summary>
public class LoadCheckpoint : MonoBehaviour
{
    public void Active()
    {
        SaveLoadManager.Instance.LoadCheckpointData();
    }
}
