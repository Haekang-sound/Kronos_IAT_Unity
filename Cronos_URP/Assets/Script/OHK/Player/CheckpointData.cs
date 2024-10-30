using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewCheckpointData", menuName = "Game/Checkpoint Data")]
public class CheckpointData : ScriptableObject
{
    public int priority;
    public float healTP;
    public string sceneName;
    public Vector3 SpwanPos;
    public Quaternion SpwanRot;

    private AbilityTree _abilityTree;
    private readonly string r_tpKey = "checkPoint-TP";
    private readonly string r_cpKey = "checkPoint-CP";

    public IEnumerator LoadData()
    {
        // ���� ���� �Űܾ� �ϸ� ���̵�
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            yield return SceneController.Instance.StartCoroutine(ScreenFader.FadeSceneOut(ScreenFader.FadeType.Loading));

            yield return SceneController.Instance.StartCoroutine(SceneLoader.Instance.LoadSceneCoroutine(sceneName));

            // �ڱ� �ڽ��� ��ȣ��
            yield return SceneController.Instance.StartCoroutine(LoadData());

            yield return SceneController.Instance.StartCoroutine(ScreenFader.FadeSceneIn(ScreenFader.FadeType.Loading));
        }
        else
        {
            /// �÷��̾�
            // üũ����Ʈ�� �̵���Ŵ
            var playerRigidbody = Player.Instance.GetComponent<Rigidbody>();
            playerRigidbody.position = SpwanPos;
            playerRigidbody.rotation = SpwanRot;

            // TP �� CP ��������
            Player.Instance.TP = PlayerPrefs.GetFloat(r_tpKey);
            Player.Instance.CP = PlayerPrefs.GetFloat(r_cpKey);

            // �ɷ� ���� ������ ��������
            if (_abilityTree == null)
            {
                _abilityTree = FindObjectOfType<AbilityTree>();
            }

            _abilityTree.LoadData(SaveLoadManager.Purpose.checkpoint.ToString());
        }
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

        _abilityTree.SaveData(SaveLoadManager.Purpose.checkpoint.ToString());
    }
}
