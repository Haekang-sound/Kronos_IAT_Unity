using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    // ȣ���� ���� �ؽ�Ʈ �迭
    public string[] RegionNames;
    
    // ȣ���� ����Ʈ �ؽ�Ʈ �迭
    public string[] QuestLines;

    // ����Ʈ �޼� �迭
    [SerializeField]
    private bool curQuesting;

    UIManager uiManager;

    // �Ǳ���
    private static QuestManager instance;
    public static QuestManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<QuestManager>();
                if (instance == null)
                {
                    GameObject qManager = new GameObject(typeof(QuestManager).Name);
                    instance = qManager.AddComponent<QuestManager>();

                    DontDestroyOnLoad(qManager);
                }
            }
            return instance;
        }
    }

    // ����Ʈ ��ǥ�� ȣ���ϴ� �Լ�
    // �ε�����°�� �ؽ�Ʈ�� �迭���� �ҷ��� ��� ��
    public IEnumerator CallingQuest(int idx)
    {
        // ���� ����Ʈ�� ������ ����ä�� �� ��ǥ�� �ҷ��´ٸ� ���� �ִϸ��̼�
        if (curQuesting)
        {
            Debug.Log("��ǥ �� ���ο� ��ǥ ȣ���");
            yield return uiManager.StartCoroutine(uiManager.FailSubObjective());
        }

        // ���� ����Ʈ�� �޼��� �Ǿ��ٸ� (!curQuesting)�̶��
        // ���� ��ǥ��
        //uiManager.StartCoroutine(uiManager.AppearMainObjective(idx));
        uiManager.StartAppearMain(idx);
    }

    // ���� ��ǥ �������Դϴ�.
    public void Questing()
    {
        curQuesting = true;
    }

    // ���� �������� ��ǥ�� �޼��ߵ� �ƴϵ� ���½��ϴ�.
    public void QuestDone()
    {
        // ui�Ŵ������� �θ�����
        curQuesting = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        uiManager = UIManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        // 0�� ������ ����Ʈ �޼�
        if (Input.GetKeyDown(KeyCode.Alpha0))
            uiManager.Achieve();
    }
}
