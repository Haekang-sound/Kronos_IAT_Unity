using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_UpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    , IPointerDownHandler
{
    public bool isSelected;
    Button thisButton;
    UI_UpgradePopup upgradeButton;

    private void Awake()
    {
        isSelected = false;
        thisButton = GetComponent<Button>();
        upgradeButton = FindObjectOfType<UI_UpgradePopup>();

        thisButton.onClick.AddListener(OnClick);
    }

    // 이 버튼에 마우스 포인터가 호버 중이라면
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Now Pointing {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exit Pointing {gameObject.name}");
    }

    // 이 버튼을 클릭했다면
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Click {gameObject.name}");
    }

    void OnClick()
    {
        upgradeButton.OnButtonClick(thisButton);
    }
}
