using UnityEngine;

/// <summary>
/// �÷��� ���� ����� �����͸� ȣ���ϴ� Ŭ�����Դϴ�.
/// </summary>
public class LoadCheckpoint : MonoBehaviour
{
    public void Active()
    {
        SaveLoadManager.Instance.LoadCheckpointData();
    }
}
