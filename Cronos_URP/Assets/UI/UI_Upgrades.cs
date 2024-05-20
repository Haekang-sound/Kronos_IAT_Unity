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
            Debug.Log("�÷��̾ ����");
        }
    }

    // ���߿� �Ű������� �߰��Ǿ� ������ ��
    public void UpgradeB()
    {
        // ���� : �ִ� �ð� 20% ����
        float val = player.maxTP * 0.2f;
        player.AdjustTP(val);
    }

    public void UpgradeS()
    {
        // ���� : �̵��ӵ� 30% ����
        float val = player.moveSpeed * 0.3f;
        player.AdjustSpeed(val);
    }

    public void UpgradeG()
    {
        // ���� : ���ݷ� 50% ����
        float val = player.Damage * 1.5f;
        player.AdjustAttackPower(val);
    }
}
