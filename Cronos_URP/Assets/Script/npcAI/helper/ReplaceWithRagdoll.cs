using UnityEngine;

/// <summary>
/// �� Ŭ������ ���� ��ü�� ��ü�Ͽ� 'Ragdoll' �������� �����ϴ� ������ �մϴ�.
/// </summary>
public class ReplaceWithRagdoll : MonoBehaviour
{
    public GameObject ragdollPrefab;

    public void Replace()
    {
        GameObject ragdollInstance = Instantiate(ragdollPrefab, transform.position, transform.rotation);

        ragdollInstance.SetActive(true);
    }
}
