using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų Ʈ���� �˾�â�� �����ϸ�, 
/// �˾� �޽����� ��ư ������ �����ϰ� Ȯ�� �� ��� ��ư�� ������ ó���մϴ�.
/// </summary>
public class PopupController : MonoBehaviour
{
    public GameObject popupPanel; // �˾�â �г�
    public TMP_Text popupText; // �˾�â �ؽ�Ʈ
    public Button confirmButton; // Ȯ�� ��ư
    public Button cancelButton; // ��� ��ư

    private System.Action onConfirmAction; // Ȯ�� �� ������ �׼�
    private System.Action onCancelAction; // ��� �� ������ �׼�

    void Start()
    {
        // ó���� �˾�â�� ����ϴ�.
        popupPanel.SetActive(false);
    }

    // �˾�â�� ���� �޽����� ��ư ���� ����
    public void OpenPopup(string message, System.Action onConfirm, System.Action onCancel = null)
    {
        popupText.text = message;
        onConfirmAction = onConfirm;
        onCancelAction = onCancel;

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(OnConfirm);

        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(OnCancel);

        popupPanel.SetActive(true);
    }

    // �˾�â �ݱ�
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

    // Ȯ�� ��ư Ŭ�� �� ȣ��
    private void OnConfirm()
    {
        onConfirmAction?.Invoke();
        ClosePopup();
    }

    // ��� ��ư Ŭ�� �� ȣ��
    private void OnCancel()
    {
        onCancelAction?.Invoke();
        ClosePopup();
    }
}
