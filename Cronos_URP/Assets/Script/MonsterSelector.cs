using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MonsterSelector : MonoBehaviour
{

    public Transform Monsetr0;
    public Transform Monsetr1;
    public Transform Monsetr2;
    public Transform Monsetr3;

    List<Transform> monsters;

    // ���� Ÿ�ٿ� �װ� ������
    public AutoTargetting autoTargetor;

    // Start is called before the first frame update
    void Start()
    {
        // �ϴ� ���
        monsters.Add(Monsetr0);
        monsters.Add(Monsetr1);
        monsters.Add(Monsetr2);
        monsters.Add(Monsetr3);
    }

    // Update is called once per frame
    void Update()
    {
        Transform _temp;

        // ���� ����� �ָ� ã�´�
        for (int i = 0; i < monsters.Count; i++) 
        {
            Vector3 min = autoTargetor.PlayerObject.position - monsters[i].position;
            // ���ͻ����� ���͸� ���ϰ� ũ�⸦ ���ؼ� ���� ���� ���͸� ã�Ƽ� �־�����
            
            // �þ߹����� ���� �ϸ� ���� ���� ������ �Ұ��� �Ӹ��� �ȵ��Ƽ�
        }

        // ����Ÿ�ٿ� �־��ش�
        //autoTargetor.Target = _temp;


    }
}
