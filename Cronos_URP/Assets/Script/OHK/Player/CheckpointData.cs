using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewCheckpointData", menuName = "Game/Checkpoint Data")]
public class CheckpointData : ScriptableObject
{
    public int priority;
    public float healTP;
    public string sceneName;
    public Transform SpwanPos;

    private AbilityTree _abilityTree;
    private readonly string r_tpKey = "checkPoint_TP";
    private readonly string r_cpKey = "checkPoint_CP";

    public void LoadData()
    {
        // 만일 씬을 옮겨야 하면 씬이동
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            SceneController.TransitionToScene(sceneName);
        }

        /// 플레이어
        // 체크포인트로 이동시킴
        var playerRigidbody = Player.Instance.GetComponent<Rigidbody>();
        playerRigidbody.position = SpwanPos.position;
        playerRigidbody.rotation = SpwanPos.rotation;

        // TP 및 CP 가져오기
        Player.Instance.TP = PlayerPrefs.GetFloat(r_tpKey);
        Player.Instance.CP = PlayerPrefs.GetFloat(r_cpKey);

        // 능력 개방 데이터 가져오기
        if (_abilityTree == null)
        {
            _abilityTree = FindObjectOfType<AbilityTree>();
        }

        _abilityTree.LoadData(SaveLoadManager.Purpose._checkpoint.ToString());
    }

    public void SaveData()
    {
        SaveLoadManager.Instance.CurrentCheckpoint = this;

        /// 플레이어
        // TP / CP 저장
        PlayerPrefs.SetFloat(r_tpKey, Player.Instance.TP);
        PlayerPrefs.SetFloat(r_cpKey, Player.Instance.CP);

        // 능력 개방 데이터 저장
        if (_abilityTree == null)
        {
            _abilityTree = FindObjectOfType<AbilityTree>();
        }

        _abilityTree.SaveData(SaveLoadManager.Purpose._checkpoint.ToString());
    }
}
