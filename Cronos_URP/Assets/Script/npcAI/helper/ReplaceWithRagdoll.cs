using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceWithRagdoll : MonoBehaviour
{
    public GameObject ragdollPrefab;

    public void Replace()
    {
        GameObject ragdollInstance = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        //// 렉돌 오브젝트를 비활성하지 않으면
        //// 해당 오브젝트의 트렌지션을 복사할 때마다 렉돌 조인트 변형/글리치 인스턴스가 발행산다.
        //ragdollInstance.SetActive(false);

        //EnemyController baseController = GetComponent<EnemyController>();

        //RigidbodyDelayedForce t = ragdollInstance.AddComponent<RigidbodyDelayedForce>();
        //t.forceToAdd = baseController.externalForce;

        //Transform ragdollCurrent = ragdollInstance.transform;
        //Transform current = transform;
        //bool first = true;

        //while (current != null && ragdollCurrent != null)
        //{
        //    if (first || ragdollCurrent.name == current.name)
        //    {
        //        // 렉돌의 자세와 현제 객체의 위치및 회전을 일치시킨다.
        //        ragdollCurrent.rotation = current.rotation;
        //        ragdollCurrent.position = current.position;
        //        first = false;
        //    }

        //    if (current.childCount > 0)
        //    {
        //        // 첫번째 자식 계층
        //        current = current.GetChild(0);
        //        ragdollCurrent = ragdollCurrent.GetChild(0);
        //    }
        //    else
        //    {
        //        while (current != null)
        //        {
        //            if (current.parent == null || ragdollCurrent.parent == null)
        //            {
        //                // 해당 객체를 찾을 수 없을 때
        //                current = null;
        //                ragdollCurrent = null;
        //            }
        //            else if (current.GetSiblingIndex() == current.parent.childCount - 1 ||
        //                     current.GetSiblingIndex() + 1 >= ragdollCurrent.parent.childCount)
        //            {
        //                // 트리 구조의 윗 단계로 올라가야함
        //                current = current.parent;
        //                ragdollCurrent = ragdollCurrent.parent;
        //            }
        //            else
        //            {
        //                // 다음 반복을 위해 트리 구조의 같은 레벨을 찾는다
        //                current = current.parent.GetChild(current.GetSiblingIndex() + 1);
        //                ragdollCurrent = ragdollCurrent.parent.GetChild(ragdollCurrent.GetSiblingIndex() + 1);
        //                break;
        //            }
        //        }
        //    }
        //}


        ragdollInstance.SetActive(true);

        if (this.CompareTag("Player"))
        {
            var camera = GameObject.FindGameObjectWithTag("PlayerFollowCamera");
            var cameraRoot = GameObject.FindGameObjectWithTag("CinemachineTarget");


            // ragdollInstance 게임 오브젝트의 자식 게임 오브젝트를 모두 가져옵니다.
            Transform[] children = ragdollInstance.GetComponentsInChildren<Transform>();

            // 가져온 자식 게임 오브젝트를 순회하며 CinemachineTarget 태그를 가진 것을 찾습니다.
            foreach (Transform child in children)
            {
                if (child.CompareTag("CinemachineTarget"))
                {
                    camera.GetComponent<CinemachineVirtualCamera>().Follow = child;

                    break;
                }
            }

        }

        Destroy(gameObject);
    }
}
