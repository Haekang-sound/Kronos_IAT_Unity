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

        GetText((int)Texts.BronzeText).text = "바인드 테스트를 하면 이렇게 텍스트를 코드에서 바꿀 수 있다.";
        GetText((int)Texts.SilverTitle).text = "실버 바인드 제목";
        GetImage((int)Images.GoldImage).sprite = Resources.Load<Sprite>("UI/power4");
    }
}
