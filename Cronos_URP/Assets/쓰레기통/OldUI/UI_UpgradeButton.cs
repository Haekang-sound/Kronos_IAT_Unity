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

    // �� ��ư�� ���콺 �����Ͱ� ȣ�� ���̶��
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log($"Now Pointing {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log($"Exit Pointing {gameObject.name}");
    }

    // �� ��ư�� Ŭ���ߴٸ�
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Click {gameObject.name}");
    }

    void OnClick()
    {
        upgradeButton.OnButtonClick(thisButton);
    }
}
