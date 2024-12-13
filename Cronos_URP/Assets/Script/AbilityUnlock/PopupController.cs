using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 스킬 트리의 팝업창을 관리하며, 
/// 팝업 메시지와 버튼 동작을 설정하고 확인 및 취소 버튼의 동작을 처리합니다.
/// </summary>
public class PopupController : MonoBehaviour
{
    public GameObject popupPanel; // 팝업창 패널
    public TMP_Text popupText; // 팝업창 텍스트
    public Button confirmButton; // 확인 버튼
    public Button cancelButton; // 취소 버튼

    private System.Action onConfirmAction; // 확인 시 실행할 액션
    private System.Action onCancelAction; // 취소 시 실행할 액션

    void Start()
    {
        // 처음엔 팝업창을 숨깁니다.
        popupPanel.SetActive(false);
    }

    // 팝업창을 열고 메시지와 버튼 동작 설정
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

    // 팝업창 닫기
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }

    // 확인 버튼 클릭 시 호출
    private void OnConfirm()
    {
        onConfirmAction?.Invoke();
        ClosePopup();
    }

    // 취소 버튼 클릭 시 호출
    private void OnCancel()
    {
        onCancelAction?.Invoke();
        ClosePopup();
    }
}
