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
    private readonly string k_tpKey = "checkPoint_TP";
    private readonly string k_cpKey = "checkPoint_CP";

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
        Player.Instance.TP = PlayerPrefs.GetFloat(k_tpKey);
        Player.Instance.CP = PlayerPrefs.GetFloat(k_cpKey);

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
        PlayerPrefs.SetFloat(k_tpKey, Player.Instance.TP);
        PlayerPrefs.SetFloat(k_cpKey, Player.Instance.CP);

        // �ɷ� ���� ������ ����
        if (_abilityTree == null)
        {
            _abilityTree = FindObjectOfType<AbilityTree>();
        }

        _abilityTree.SaveData(SaveLoadManager.Purpose._checkpoint.ToString());
    }
}
