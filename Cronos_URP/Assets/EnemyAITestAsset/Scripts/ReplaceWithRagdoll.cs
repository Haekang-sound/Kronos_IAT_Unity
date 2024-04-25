using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceWithRagdoll : MonoBehaviour
{
    public GameObject ragdollPrefab;

    public void Replace()
    {
        GameObject ragdollInstance = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        // ���� ������Ʈ�� ��Ȱ������ ������
        // �ش� ������Ʈ�� Ʈ�������� ������ ������ ���� ����Ʈ ����/�۸�ġ �ν��Ͻ��� ������.
        ragdollInstance.SetActive(false);

        EnemyController baseController = GetComponent<EnemyController>();

        RigidbodyDelayedForce t = ragdollInstance.AddComponent<RigidbodyDelayedForce>();
        t.forceToAdd = baseController.externalForce;

        Transform ragdollCurrent = ragdollInstance.transform;
        Transform current = transform;
        bool first = true;

        while (current != null && ragdollCurrent != null)
        {
            if (first || ragdollCurrent.name == current.name)
            {
                // ������ �ڼ��� ���� ��ü�� ��ġ�� ȸ���� ��ġ��Ų��.
                ragdollCurrent.rotation = current.rotation;
                ragdollCurrent.position = current.position;
                first = false;
            }

            if (current.childCount > 0)
            {
                // ù��° �ڽ� ����
                current = current.GetChild(0);
                ragdollCurrent = ragdollCurrent.GetChild(0);
            }
            else
            {
                while (current != null)
                {
                    if (current.parent == null || ragdollCurrent.parent == null)
                    {
                        // �ش� ��ü�� ã�� �� ���� ��
                        current = null;
                        ragdollCurrent = null;
                    }
                    else if (current.GetSiblingIndex() == current.parent.childCount - 1 ||
                             current.GetSiblingIndex() + 1 >= ragdollCurrent.parent.childCount)
                    {
                        // Ʈ�� ������ �� �ܰ�� �ö󰡾���
                        current = current.parent;
                        ragdollCurrent = ragdollCurrent.parent;
                    }
                    else
                    {
                        // ���� �ݺ��� ���� Ʈ�� ������ ���� ������ ã�´�
                        current = current.parent.GetChild(current.GetSiblingIndex() + 1);
                        ragdollCurrent = ragdollCurrent.parent.GetChild(ragdollCurrent.GetSiblingIndex() + 1);
                        break;
                    }
                }
            }
        }


        ragdollInstance.SetActive(true);
        Destroy(gameObject);
    }
}
