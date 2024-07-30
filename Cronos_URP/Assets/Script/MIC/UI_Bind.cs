using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Bind : UI
{
    public TextMeshProUGUI bTitle;
    public TextMeshProUGUI bText;
    public TextMeshProUGUI sTitle;
    public TextMeshProUGUI sText;
    public TextMeshProUGUI gTitle;
    public TextMeshProUGUI gText;
    public Image bImage;
    public Image sImage;
    public Image gImage;

    private void Start()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));
        Bind<Image>(typeof(Images));

        GetText((int)Texts.BronzeText).text = "���ε� �׽�Ʈ�� �ϸ� �̷��� �ؽ�Ʈ�� �ڵ忡�� �ٲ� �� �ִ�.";
        GetText((int)Texts.SilverTitle).text = "�ǹ� ���ε� ����";
        GetImage((int)Images.GoldImage).sprite = Resources.Load<Sprite>("UI/power4");
    }
}
