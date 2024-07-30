using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_PointText : UI_TPCP
{
    [SerializeField]
    TextMeshProUGUI textTP;
    //public TextMeshProUGUI textCP;

    Player player;
    float tp;
    //float cp;

    private void Start()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.Log("플레이어가 없당께요");
        }
    }

    private void Update()
    {
        tp = player.TP;
        //cp = player.CP;
        textTP.text = tp.ToString("000");
        //textCP.text = "CP:" + cp.ToString("0000");
    }
}
