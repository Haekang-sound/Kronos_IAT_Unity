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
        // ���� ���� �Űܾ� �ϸ� ���̵�
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            SceneController.TransitionToScene(sceneName);
        }

        /// �÷��̾�
        // üũ����Ʈ�� �̵���Ŵ
        var playerRigidbody = Player.Instance.GetComponent<Rigidbody>();
        playerRigidbody.position = SpwanPos.position;
        playerRigidbody.rotation = SpwanPos.rotation;

        // TP �� CP ��������
        Player.Instance.TP = PlayerPrefs.GetFloat(r_tpKey);
        Player.Instance.CP = PlayerPrefs.GetFloat(r_cpKey);

        // �ɷ� ���� ������ ��������
        if (_abilityTree == null)
        {
            _abilityTree = FindObjectOfType<AbilityTree>();
        }

        _abilityTree.LoadData(SaveLoadManager.Purpose._checkpoint.ToString());
    }

    public void SaveData()
    {
        SaveLoadManager.Instance.CurrentCheckpoint = this;

        /// �÷��̾�
        // TP / CP ����
        PlayerPrefs.SetFloat(r_tpKey, Player.Instance.TP);
        PlayerPrefs.SetFloat(r_cpKey, Player.Instance.CP);

        // �ɷ� ���� ������ ����
        if (_abilityTree == null)
        {
            _abilityTree = FindObjectOfType<AbilityTree>();
        }

        _abilityTree.SaveData(SaveLoadManager.Purpose._checkpoint.ToString());
    }
}
