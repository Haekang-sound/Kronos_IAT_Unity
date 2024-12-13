using UnityEngine;

/// <summary>
/// 이 클래스는 기존 객체를 대체하여 'Ragdoll' 프리팹을 생성하는 역할을 합니다.
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
