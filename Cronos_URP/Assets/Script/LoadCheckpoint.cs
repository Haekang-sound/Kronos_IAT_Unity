using UnityEngine;

public class LoadCheckpoint : MonoBehaviour
{
    public void Active()
    {
        SaveLoadManager.Instance.LoadCheckpointData();
    }
}
