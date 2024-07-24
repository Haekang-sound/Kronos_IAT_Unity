using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Upgrades : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("플레이어가 없다");
        }
    }

    // 나중엔 매개변수가 추가되어 구현될 것
    public void UpgradeB()
    {
        // 데모 : 최대 시간 20% 증가
        float val = player.MaxTP * 0.2f;
        player.AdjustTP(val);
    }

    public void UpgradeS()
    {
        // 데모 : 이동속도 30% 증가
        float val = player.moveSpeed * 0.3f;
        player.AdjustSpeed(val);
    }

    public void UpgradeG()
    {
        // 데모 : 공격력 50% 증가
        float val = player.CurrentDamage * 1.5f;
        player.AdjustAttackPower(val);
    }
}
